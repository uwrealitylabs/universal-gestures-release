# Universal Gestures

Repository for the Universal Gestures package. Easily train and use AI models to detect any
hand gesture you can imagine in VR.

# Installation

Getting started is simple.

1. In your project, open your package manager (click Window > Package Manager).
2. Click the + button in the top left and select "Add package from git URL".
3. Install the [Barracuda](https://github.com/Unity-Technologies/barracuda-release) package
   (a dependency) by pasting the following link into the url textbox:

> https://github.com/Unity-Technologies/barracuda-release.git

4. Install the Universal Gestures library by following step 2 and pasting the following link into the url textbox:

> https://github.com/uwrealitylabs/universal-gestures-release.git

You are now ready to use the Universal Gestures library!

# Usage

## Using a model detect gestures

To get started with inference:

1. Search for the GestureInferenceOneHand or GestureInferenceTwoHands
   prefabs to detect one-handed or two-handed gestures, respectively.
2. Fill in the required parameters to the script, including the Meta SDK hand objects/features, the
   model trained on your desired gesture (try the installable samples or create your own as
   described below), the desired function to run, and configuration such as inference frequency and
   activation confidence threshold.

That's it! Now, your function should run whenever you make the
target hand gesture.

## Creating your own model

With the Universal Gestures library, training your own model to detect any hand gesture is easy.

1. Install and open the "Training Scene" sample in the package manager under the Universal Gestures library.
2. Run the scene.
3. Select either the "One Hand" or "Two Hands" button depending on what kind of data you're collecting.
4. To begin recording, select either the "Rec. Pos." or "Rec. Neg." button to record
   positive or negative gesture data, respectively. Configure the delay before recording begins
   and how long the recording lasts in the Unity project "Scripts" GameObject by changing the
   "Recording Duration" and "Recording Start Delay" fields.
5. (TODO: upon finishing trianing pipeline, complete this documentation)
 <!-- 5. The data is saved to the `JsonData/` folder in the root of the Unity project
    if on running on computer, or in `Root/Android/data/{project name}/files/JsonData` when
    recording standalone on a headset. -->
