# Universal Gestures

Seamlessly use AI to detect any hand gesture you can imagine in VR.

# Setup

1. In your Unity project, open your package manager (click Window > Package Manager).
2. Click the + button in the top left and select "Add package from git URL".
3. Install the [Barracuda](https://github.com/Unity-Technologies/barracuda-release) package
   (a dependency) by pasting the following link into the url textbox:

> https://github.com/Unity-Technologies/barracuda-release.git

4. Install the Universal Gestures library by following step 2 and pasting the following link into the url textbox:

> https://github.com/uwrealitylabs/universal-gestures-release.git

5. In the package manager, find the installed Universal Gestures package, click on the Samples tab,
   and install the Sample Models and Training Scene.

You are now ready to use the Universal Gestures library!

# Usage

## Getting Started

Watch the [**Getting Started demo**](https://www.youtube.com/watch?v=daMcOk1g8BM&list=PLOsZCVyu97OQ65r99WXG5chM_k2SpovKT)
or reference the steps below.

1. Search for the `UGDataExtractor` and `UGInferenceRunner` prefabs and drag both into your project.
2. Fill in the UGDataExtractorScript data source fields with the relevant Hand and OVRHand scripts to read data from.
3. Fill in the UGInferenceRunnerScript setup fields with the target gesture model, the hand mode, and data extractor.
   Additionally, specify a function to run on gesture detection. Configure the other fields (ex. activation confidence threshold, inference interval) as applicable.

That's it! Now, your function should run whenever you make the target hand gesture.
Note that multiple UGInferenceRunners can exist and run inference/functions on different gestures simultaneously,
while all reading from a single UGDataExtractor.

## Creating a Custom Gesture

Watch the [**Creating a Custom Gesture demo**](https://www.youtube.com/watch?v=jDjiCyg7XCA&list=PLOsZCVyu97OQ65r99WXG5chM_k2SpovKT&index=2)
or reference the steps below.

1. Clone the [universal-gestures-lab repository](https://github.com/uwrealitylabs/universal-gestures-lab).
2. With Python 3.12.3 or later installed, install the dependencies of universal-gestures-lab repository
   (i.e., run `pip install -r requirements.txt` in the root of the repository, virtual environment recommended).
3. Run the `run_training_server.py` script. Copy the server hosting address. Note, the headset and server must be
   running on the same wifi network so they can communicate.
4. Back in your Unity project, navigate to Package Manager > Universal Gestures > Samples and install the Training Scene.
5. Open the Training Scene.
6. Locate the UGDataWriter at the top level of the Hierarchy, and replace the `Uri` field with the server's hosting address that you copied,
   adding a forward slash at the end so the address ends with a slash.
   Configure other settings as desired, such as Gesture Name or Recording Duration.
7. Open Edit > Project Settings > Player > Other Settings and set "Allow downloads over HTTP"
   to "Always allowed". Once finished these steps, feel free to revert this setting to "Not allowed"
   for security purposes.
8. Run the Training Scene on your VR headset. In the scene, you will see three panels
   (from left to right, the current UGInferenceRunner model's inference output, the current hand data, and the recording status),
   as well as a control panel with buttons.
9. Select the desired gesture type (left hand, right hand, or two hands) from the control panel.
10. Select either the "Rec. Pos." or "Rec. Neg." button to begin recording positive or negative gesture data, respectively.
    Ensure to be thorough, especially with the negative data (any hand gestures not recorded as "negative" may cause false
    positives in the model, even if not present in the positive data).
    Aim for at least 30 seconds of positive recordings and 60 seconds of negative recordings.
11. Once finished collecting data, press the "Send Data to Server".
    On right-most panel, you will see "Model loaded!" once the new model has finished training and has loaded into the scene.
    You will be able to test your model's performance by making the hand gesture and seeing the model's confidence in the left-most panel.
12. To save your model, exit the scene and navigate to the universal-gestures-lab repository's `trained_model/` directory.
    Drag the relevant model's `.onnx` file into your Unity project, and attach it to a UGInferenceRunner for use.

# Contact/Contributing

Universal Gestures is a project by Waterloo Reality Labs.
Visit the
[Waterloo Reality Labs website](https://uwrealitylabs.com/)
to get in touch with us and learn more about our other projects.

The source code for the Unity side of this project can be found in our
[universal-gestures-unity](https://github.com/uwrealitylabs/universal-gestures-unity)
repository.

The source code for the AI side of this project can be found in our
[universal-gestures-lab](https://github.com/uwrealitylabs/universal-gestures-lab)
repository.
