# Implement `PanTiltZoomCamera` Public Class in C#

## Constructors
1. Accepts the camera's ID and stores it into a private variable associated with the class
2. Assumes ID = 1
3. Accepts a set of Pan, Tilt, and Zoom speeds stored into private variables

## Methods

The class will contain a set of public methods that return strings, implementing the [VISCA command set for controlling PTZ cameras](https://www.epiphan.com/userguides/LUMiO12x/Content/UserGuides/PTZ/3-operation/VISCAcommands.htm).

### Pan_tiltDrive

- `string Up(ID, Speed) {}`
- `string Down(ID, Speed) {}`
- `string Left(ID, Speed) {}`
- `string Right(ID, Speed) {}`
- `string UpLeft(ID, PanSpeed, TiltSpeed) {}`
- `string UpRight(ID, PanSpeed, TiltSpeed) {}`
- `string DownLeft(ID, PanSpeed, TiltSpeed) {}`
- `string DownRight(ID, PanSpeed, TiltSpeed) {}`
- `string Stop_PanTilt(ID) {}`

note that the speeds vary depending on pan, tilt or zoom speed possible values, in hexadecimal -- ranges are given

### CAM_Zoom
- `string Zoom_In(ID, Speed) {}` [Tele, Standard]
- `string Zoom_Out(ID, Speed) {}` [Wide, Standard]
- `string Stop_Zoom(ID) {}`

### CAM_Memory
- `string Store_Preset(ID, Preset_Number) {}`
- `string Recall_Preset(ID, Preset_Number) {}`

### CAM_Power
- `string Power_On(ID) {}`
- `string Power_Off(ID) {}`
- `string Power_Inquiry(ID) {}` (for this one, see CAM_PowerInq)

Example: `\x81\x01\x04\x00\x02\xFF` would be a Power On command for a camera at ID #1. Note that these are bytes with no spaces!


### `Handle_Response`

A feedback-parsing method called `Handle_Response` that will interpret acknowledgement, command execution, and power inquiry feedback. Use a boolean named `Power_Is_On` to be set to true when the power inquiry returns true, and false when false.  It will need to gather bytes together into a buffer up until it finds the first `\xFF` character and then interpret the response from there (and remove interpreted responses from the buffer). The buffer should also have some safety mechanism to clear itself if it gets "too big" (let's say > 100 chars) to avoid memory overruns.

Note the ID change in the responses also -- `y = x + 8`! So an `x = 1` would show `\x90\x50\x02\xFF` for example.

### Overloaded Methods

Overloaded methods that do not need the speed variable and instead use the provided ones in the constructor.