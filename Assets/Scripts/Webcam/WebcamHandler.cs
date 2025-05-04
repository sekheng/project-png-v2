using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamHandler : MonoBehaviour
{

    public static WebcamHandler instance = null;

    // reference to Trapeziums Manager
    [SerializeField] private TrapeziumsManager trapeziumsManager;
    public TrapeziumsManager TrapeziumsManager
    {
        get { return trapeziumsManager; }
    }

    /*
     * Webcam texture width & height
     */
    private int width = 0;
    private int height = 0;

    [SerializeField] private int requestedWidth = 160;
    [SerializeField] private int requestedHeight = 120;
    [SerializeField, Tooltip("chosen webcam device name")]
    private string webcamDeviceName;

    //public int colorThreshold = 25;

    private WebCamTexture webcamTexture = null;
    private bool webcamActive = false;

    private Color32[] prevPixelArray = null;

    private Texture2D imageTexture; // for image/material texture

    private bool textureLoaded = false;
    public bool IsTextureLoaded() { return textureLoaded; }

    /*
     * Min and max of webcam mask (i.e. the bounding box for possible pixels)
     * (Currently not in use)
     */
    /// width 0 to max is right to left; *left to right?
    /// height 0 to max is bottom to top
    public int minWidth { get; private set; }
    public int maxWidth { get; private set; }
    public int minHeight { get; private set; }
    public int maxHeight { get; private set; }
    private bool maskWrittenBefore = false;

    // flip
    //private bool horizontalFlip = false;
    //private bool verticalFlip = false;

    public int GetWidth() { return width; }
    public int GetHeight() { return height; }
    public WebCamTexture GetWebcamTexture() { return webcamTexture; }
    public Texture2D GetWebcamTextureRender() { return imageTexture; }

    // for normalization calculation
    //private Vector3 n0, n1, n2, n3;

    // Use this for initialization
    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        webcamDeviceName = PlayerPrefs.GetString(WebcamDropdown.CONST_WEBCAMNAME);
        minWidth = 0;
        maxWidth = 160;
        minHeight = 0;
        maxHeight = 120;

        ActivateWebcam();
    }

    private void Start()
    {
        WebcamInputReceiver.instance.SetInputWidth(maxWidth - minWidth);
        WebcamInputReceiver.instance.SetInputHeight(maxHeight - minHeight);
    }

    private void OnDisable()
    {
        DeactivateWebcam();
    }

    public bool IsWebcamActive()
    {
        return webcamActive;
    }

    public void ActivateWebcam()
    {
        if (webcamTexture)
        {
            webcamTexture.Stop();
            Destroy(webcamTexture);
            webcamTexture = null;
        }
        if (string.IsNullOrEmpty(webcamDeviceName))
        {
            webcamTexture = new WebCamTexture(WebCamTexture.devices[0].name, requestedWidth, requestedHeight, 30);
        }
        else
        {
            webcamTexture = new WebCamTexture(webcamDeviceName, requestedWidth, requestedHeight, 30);
        }
        webcamTexture.Play();
        webcamActive = true;
    }

    public void DeactivateWebcam()
    {
        if (webcamTexture != null)
        {
            webcamTexture.Stop();
            webcamActive = false;
        }
    }

    public bool DidUpdateThisFrame()
    {
        return webcamTexture.didUpdateThisFrame;
    }

    public void Update()
    {
        // If webcam updated in this frame, process the new webcam data
        if (!webcamTexture.didUpdateThisFrame)
            return;

        // If the webcam texture info has not been obtained before, update it
        if (!textureLoaded)
        {
            width = webcamTexture.width;
            height = webcamTexture.height;

            // load trapeziums data
            trapeziumsManager.LoadTrapeziumsData();

            //Debug.Log("width: " + width);
            //Debug.Log("height: " + height);

            // update the webcam mask info
            if (!maskWrittenBefore) // whether the webcam mask has been written to before
            {
                // for now, it is just the entire screen
                minWidth = 0;
                minHeight = 0;
                maxWidth = width;
                maxHeight = height;

                // TODO: when does maskWrittenBefore get updated?
                // also, will this get updated when trapezium data gets modified?
            }

            // Create a Texture2D for the webcam image
            imageTexture = new Texture2D(width, height);
            textureLoaded = true;   // webcam texture info has been obtained & updated
        }

        // Get pixels from texture
        //Texture2D texture = (Texture2D)rawImage.texture;
        Color32[] pixelArray = webcamTexture.GetPixels32();

        // If it is not the first frame, check for changes in pixels
        if (prevPixelArray != null) // no previous pixel array available; i.e. not the first frame
        {
            // Make a copy of the pixel array to allow editing
            Color32[] editedPixelArray = (Color32[])pixelArray.Clone();  // to edit the video's pixels

            // Clear blob/input list before processing this frame's data
            BlobData.blobs.Clear(); // clear list of blobs away
            WebcamInputReceiver.instance.ClearInputList();  // clear crosses & red blobs list as it should only store those that exist this frame

            /***************************
             * Process pixels in array
             ***************************/
            // Loop through pixel array to process each pixel
            //for (int posX = minWidth; posX < maxWidth; ++posX)   // Position of X pixel in final texture output
            for (int posX = 0; posX < width; ++posX)   // Position of X pixel in final texture output
            {
                // Get x-index of pixel to process
                int idxX = posX;    // get x-index for pixel
                                    // If horizontalFlip setting is true, take the flipped corresponding index
                if (CalibrationData.instance.horizontalFlip)    // horizontal flip
                    idxX = width - posX - 1;    // apply flip

                // Loop through pixel array; to process each pixel
                //for (int posY = minHeight; posY < maxHeight; ++posY) // Position of Y pixel in final texture output
                for (int posY = 0; posY < height; ++posY) // Position of Y pixel in final texture output
                {
                    // Calculate the actual pixel position
                    int pixelPos = posY * width + posX; // index position in final texture output

                    // Check if point is inside trapezium or not; if point is outside trapezium, skip processing this point
                    //if (IsPointOutsideQuad(posX, posY))
                    if (trapeziumsManager.IsPointOutsideTrapeziums(posX, posY))
                    {
                        editedPixelArray[pixelPos] = new Color32(0, 0, 0, 1);   // set the pixel to black
                        continue;
                    }

                    // Get y-index of pixel to process
                    int idxY = posY;    // get y-index for pixel
                                        // If verticalFlip setting is true, take the flipped corresponding index
                    if (CalibrationData.instance.verticalFlip)  // vertical flip
                        idxY = height - posY - 1;   // apply flip

                    // Get the pixel to process
                    int pixelIdx = idxY * width + idxX; // index of pixel to use for calculation

                    // If there is a flip, re-assign pixel to pixel array (as pixel would have changed)
                    if (CalibrationData.instance.horizontalFlip || CalibrationData.instance.verticalFlip)
                        editedPixelArray[pixelPos] = pixelArray[pixelIdx];

                    // Get the colour distance (squared), i.e. difference in colour from the previous frame's pixel in the same pos
                    float colorDistSqr = Utility.DistSqr(prevPixelArray[pixelIdx].r, pixelArray[pixelIdx].r,
                                                prevPixelArray[pixelIdx].g, pixelArray[pixelIdx].g,
                                                prevPixelArray[pixelIdx].b, pixelArray[pixelIdx].b);
                    // Check if the colour distance (squared) exceeds the threshold
                    // If it exceeds, it means the pixel at that position has changed greatly, and thus there was motion
                    if (colorDistSqr > CalibrationData.instance.colorThresholdSqr)  // this is a moving pixel
                    {
                        // If it is near a blob, save it into a blob
                        bool addedToBlob = false;
                        foreach (BlobData blob in BlobData.blobs)
                        {
                            if (blob.IsNear(posX, posY) && blob.Size() < CalibrationData.instance.maxBlobSize)
                            {
                                blob.Add(posX, posY);
                                addedToBlob = true;
                                break;
                            }
                        }

                        // If not near any blob, create a new blob for it
                        if (!addedToBlob)
                        {
                            BlobData newBlob = new BlobData(posX, posY);
                            BlobData.blobs.Add(newBlob);
                        }
                    }
                }
            }   /// End of pixelArray for loops

            /*****************
             * Process blobs
             *****************/
            /// Add blobs to pixels to be rendered
            // find the biggest blob
            //int blobIdx = BlobData.GetIndexOfLargestBlob();

            // If there are no blobs, skip
            if (BlobData.blobs.Count == 0)
                goto EndOfProcessingBlobs;

            // calculate acceptable blob size for red blob for this frame
            int acceptableRedBlobSize = BlobData.CalculateAcceptableRedBlobSize();

            // turn the biggest blob to red; rest to white
            for (int i = 0; i < BlobData.blobs.Count; ++i)
            {
                // Check minimum size
                if (BlobData.blobs[i].Size() < CalibrationData.instance.minimumBlobSize)    // blob is too small; ignore it
                    continue;

                //if (i == blobIdx)   // [SCRAPPED] The largest blob becomes the red blob
                //else if (BlobData.blobs[i].Size() >= acceptableRedBlobSize) // [scrapped for testing] those above acceptable red blob size become red blobs
                else    // all blobs above min. blob size become red blobs
                {
                    // Store red blobs
                    BlobData.redBlobs.Add(BlobData.blobs[i]);
                }
                // [scrapped for testing] Blobs below the acceptable red blob size will be white blobs, & crosses will not be generated
                //else
                //{
                //    // Set pixels to white
                //    for (int j = 0; j < BlobData.blobs[i].Size(); ++j)
                //    {
                //        Point pixel = BlobData.blobs[i].GetPixel(j);
                //        int pixelIdx = pixel.y * width + pixel.x;
                //
                //        // Replace the pixel in the frame with white
                //        editedPixelArray[pixelIdx].r = 255;
                //        editedPixelArray[pixelIdx].g = 255;
                //        editedPixelArray[pixelIdx].b = 255;
                //    }
                //}
            }

            //===============================================
            // render red blobs - cross skip value version
            //===============================================
            // Generate crosses for red blobs only
            for (int i = BlobData.redBlobs.Count - 1; i >= 0; --i)
            {
                // Get reference to the red blob
                BlobData redblob = BlobData.redBlobs[i];

                // Go through all pixels in red blob
                int crossSkipCount = 0; // used to track whether a cross should be generated
                for (int j = 0; j < redblob.Size(); ++j)
                {
                    // Get pixel point
                    Point pixel = redblob.GetPixel(j);
                    int pixelIdx = pixel.y * width + pixel.x;

                    // Replace the pixel in the frame with red
                    editedPixelArray[pixelIdx].r = 255;
                    editedPixelArray[pixelIdx].g = 0;
                    editedPixelArray[pixelIdx].b = 0;

                    // Add crosses when red blob first instantiated
                    if (redblob.IsFirstFrame() && crossSkipCount == j)  // if red blob's first frame, and can generate cross at this pixel
                    {
                        // Add red blob to WebcamInputReceiver - first frame only
                        WebcamInputReceiver.instance.AddRedBlob(redblob);

                        // Add cross
                        //if (CalibrationCorners.instance.IsWithinMarkers(pixel.x, pixel.y))
                        //{
                        WebcamInputReceiver.instance.AddCross(pixel);
                        //}
                        // Set next skip count value
                        crossSkipCount += 1 + CalibrationData.instance.skipValue;
                    }
                }
            }   /// End of processing red blobs

        EndOfProcessingBlobs:   // essentially, the end of the frame (after processing blob data)
            // update texture here
            UpdateImageTexture(editedPixelArray);
        }   /// End of processing previous pixel array

        // Assign prevPixelArray for next frame to reference
        prevPixelArray = (Color32[])pixelArray.Clone();
    }

    private void LateUpdate()
    {
        for (int i = 0; i < BlobData.redBlobs.Count; ++i)
        {
            if (BlobData.redBlobs[i].UpdateLifetime())  // red blob's lifespan is up
            {
                BlobData.redBlobs.RemoveAt(i);
            }
        }
    }

    private void UpdateImageTexture(Color32[] pixelArray)
    {
        imageTexture.SetPixels32(pixelArray);
        imageTexture.Apply(true);   // updateMipmaps = true
    }

    public void ChooseWebcam(string webcamName)
    {
        webcamDeviceName = webcamName;
        ActivateWebcam();
    }
}
