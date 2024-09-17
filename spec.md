# Implement `PanTiltZoomCamera` Public Class in C#

## Constructors
1. Accepts the camera's ID and stores it into a private variable associated with the class
2. Assumes ID = 1
3. Accepts a set of Pan, Tilt, and Zoom speeds stored into private variables

## Methods

The class will contain a set of public methods that return strings, implementing the [VISCA command set for controlling PTZ cameras](https://www.epiphan.com/userguides/LUMiO12x/Content/UserGuides/PTZ/3-operation/VISCAcommands.htm).

### Pan_tiltDrive

- `string Up(int id, int panSpeed) {}`
- `string Down(int id, int panSpeed) {}`
- `string Left(int id, int panSpeed) {}`
- `string Right(int id, int panSpeed) {}`
- `string UpLeft(int id, int panSpeed, int tiltSpeed) {}`
- `string UpRight(int id, int panSpeed, int tiltSpeed) {}`
- `string DownLeft(int id, int panSpeed, int tiltSpeed) {}`
- `string DownRight(int id, int panSpeed, int tiltSpeed) {}`
- `string StopPanTilt(int id) {}`

Note that the speeds vary depending on pan, tilt or zoom speed possible values, in hexadecimal -- ranges are given.

### CAM_Zoom
- `string ZoomIn(int id, int zoomSpeed) {}` - Tele(Variable)
- `string ZoomOut(int id, int zoomSpeed) {}` - Wide(Variable)
- `string StopZoom(int id) {}`

### CAM_Memory
- `string StorePreset(int id, int presetNumber) {}`
- `string RecallPreset(int id, int presetNumber) {}`

### CAM_Power
- `string PowerOn(int id) {}`
- `string PowerOff(int id) {}`
- `string PowerInquiry(int id) {}` (for this one, see CamPowerInq)

Example: `\x81\x01\x04\x00\x02\xFF` would be a Power On command for a camera at ID #1. Note that these are bytes with no spaces!

### HandleResponse

A feedback-parsing method called `HandleResponse` that will interpret acknowledgement, command execution, and power inquiry feedback. Use a boolean named `powerIsOn` to be set to true when the power inquiry returns true, and false when false. It will need to gather bytes together into a buffer up until it finds the first `\xFF` character and then interpret the response from there (and remove interpreted responses from the buffer). The buffer should also have some safety mechanism to clear itself if it gets "too big" (let's say > 100 chars) to avoid memory overruns.

Note the ID change in the responses also -- `y = x + 8`! So an `x = 1` would show `\x90\x50\x02\xFF` for example.

### Overloaded Methods

Overloaded methods that do not need the speed variable and instead use the provided ones in the constructor.