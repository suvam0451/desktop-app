# Summary

This application was presented as a means to solve simple image processing tasks such as the following 

(darknet, opencv and graphViz are open third-party source tools I used. This application is written using .NET framework and C++/CLI with interop among the above-mentioned libraries)

- Get image slices for a video at defined intervals.
- Run common client-side image cassification tasks (detection, output JSON)

The pitch was to make it look user-friendly and professional, meanwhile getting tasks done hassle-free.

I develoepd this application for my B. Tech project. I also am expected to integrate my M.Tech thesis work into this application so that it may be used by researchers in the department. Although incomplete, the following features are proposed to be added *(I am currently building CLI tools)*

- More robust Save/Load/Open features
- Ability to draw and define bounding shapes (uses in Image Detection for RoI)
- Tools to train custom models *(by integrating alexeyab/yolo-mark)* 

# Screenshots

![](./GalleryExhibiut2.png)

For each tab, The application has a **description panel** which shows usage. Most parameters are set by the **left panel** and there is an **optional image field** for visual feedback. The **right panel** is supposed to help set global variables.

**The console** in the bottom is resizable and indicates task progress with timestamps.

![](./GalleryExhibiut3.png)


The application uses MVVM pattern UI design and reuses lot of code among panels.