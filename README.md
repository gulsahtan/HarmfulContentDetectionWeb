# HarmfulContentDetectionWeb
HarmfulContentDetection web application - Application that performs real time harmful content detection on videos and images with ML.NET, ONNX.

## Preparation

Run the following commands in order
```bash
git clone https://github.com/gulsahtan/HarmfulContentDetectionWeb

```
## Web URL

- Demo application is available at https://harmfulcontentdetection.com/

- User Name: user

- User Password: user12345


## Usage

The HarmfulContentDetection web application includes the following modules:

- Image Censorship module detects the objects in the image according to the categories you have selected and presents the final version to you. After selecting the image and category, you need to press the Detect button.

- Video Management detects the objects in the video according to the categories you have selected and saves the final video to the database, allowing you to view it on the Watch Video platform. After selecting the video and category, you need to click the Detect button. Please wait until the last video on the page appears.

- Video Censorship The form application that you will download from the Real-Time Object Detection module detects the objects in real-time according to the categories you have selected in both the webcam and the uploaded video, and instantly presents each frame to you. In this application, both classic object detection and the developed method for object detection are presented together.

- Data Set module contains the data used to build the models in this application.

- Watch Video module enables the viewing of listed videos according to the selected categories.

- Demo Video  https://github.com/gulsahtan/HarmfulContentDetectionWeb/blob/main/HarmfulContentDetection.Web.Mvc/Assets/applicationvideo.mp4

## Team

- Associate Professor. Asım Sinan Yüksel: Süleyman Demirel University Computer Engineering Department, Algorithm Design, Architectural Design, Test

- Lecturer Fatma Gülşah TAN: Süleyman Demirel University Computer Engineering Department, Algorithm Design, Architectural Design, Front-End, Back-End, Test

## License

The source code is free for research and education use only. Any comercial use should get formal permission first.
