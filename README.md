# DeepCens
DeepCens is a unique tool designed to detect and censor undesirable content in real-time. It supports 4 content categories such as explicit content, alcohol, cigarette, and violence. Our system is designed to work on different platforms, inspired by video watching platforms such as Netflix and YouTube, as well as live broadcasting platforms such as Zoom and Microsoft Teams. The real-time censoring process is user-based and operates dynamically. A user can choose one of the supported content categories (or multiple categories) and the censoring process is applied in real-time accordingly. It employs an improved,fastand accurate YOLO-based algorithm with a pipeline architecture. It supports image and video files, web camera streams.

## Preparation

Run the following commands for web app in order
```bash
git clone https://github.com/gulsahtan/HarmfulContentDetectionWeb

```
How to use?

https://github.com/gulsahtan/HarmfulContentDetectionWeb/blob/main/HarmfulContentDetection.Web.Mvc/Assets/web.mp4

Run the following commands for in order (with pipeline)
```bash
git clone https://github.com/gulsahtan/HarmfulContentDetectionRealTimeRepo
```

How to use?

https://github.com/gulsahtan/HarmfulContentDetectionWeb/blob/main/HarmfulContentDetection.Web.Mvc/Assets/withpipeline.mp4

Run the following commands for in order (without pipeline)
```bash
git clone https://github.com/gulsahtan/HarmfulContentDetectionDesktop
```

How to use?

https://github.com/gulsahtan/HarmfulContentDetectionWeb/blob/main/HarmfulContentDetection.Web.Mvc/Assets/withoutpipeline.mp4

## Web URL

- Demo application is available at https://harmfulcontentdetection.com/

- User Name: user

- User Password: user12345

## Technologies and Tools

* **Front-end:** CSS, JQuery, JavaScript
* **Back-end:** C#
* **Database:** MsSql
* **Data Modeling:** Google Colab (Yolov5) 
* **Model Viewer:** NETRON
* **Model:** ONNX
* **Authentication:** Jwt
* **IDE:** Visual Studio
* **Version Control System:** Git

## Usage

The HarmfulContentDetection web application includes the following modules:

-Login 

- User Name: user

- User Password: user12345

![image](images/login.PNG)

- Image Censorship module detects the objects in the image according to the categories you have selected and presents the final version to you. After selecting the image and category, you need to press the Detect button.

![image](images/imagecensorship.PNG)

- Video Management detects the objects in the video according to the categories you have selected and saves the final video to the database, allowing you to view it on the Watch Video platform. After selecting the video and category, you need to click the Detect button. Please wait until the last video on the page appears.

![image](images/videomanagement.PNG)

- Video Censorship The form application that you will download from the Real-Time Object Detection module detects the objects in real-time according to the categories you have selected in both the webcam and the uploaded video, and instantly presents each frame to you. In this application, both classic object detection and the developed method for object detection are presented together.

![image](images/videocensorship.PNG)

- Data Set module contains the data used to build the models in this application.

![image](images/dataset.PNG)

- Watch Video module enables the viewing of listed videos according to the selected categories.

![image](images/watch.PNG)

![image](images/watch2.PNG)

## Team

- Associate Professor. Asım Sinan Yüksel: Süleyman Demirel University Computer Engineering Department, Algorithm Design, Architectural Design, Test

- Fatma Gülşah TAN: Süleyman Demirel University Computer Engineering Department, Algorithm Design, Architectural Design, Front-End, Back-End, Test

## License

The source code is free for research and education use only. Any comercial use should get formal permission first.
