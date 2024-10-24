# Spec

## `PanTiltZoomCamera` Class

### Constructors

1. Accepts the camera's ID and stores it into a private variable associated with the class
2. Assumes ID = 1
3. Accepts a set of Pan, Tilt, and Zoom speeds stored into private variables

### Methods

The class will contain a set of public methods that return strings, implementing the [VISCA command set for controlling PTZ cameras](https://www.epiphan.com/userguides/LUMiO12x/Content/UserGuides/PTZ/3-operation/VISCAcommands.htm).

#### Pan_tiltDrive

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

#### CAM_Zoom

- `string ZoomIn(int id, int zoomSpeed) {}` - Tele(Variable)
- `string ZoomOut(int id, int zoomSpeed) {}` - Wide(Variable)
- `string StopZoom(int id) {}`

#### CAM_Memory

- `string StorePreset(int id, int presetNumber) {}`
- `string RecallPreset(int id, int presetNumber) {}`

#### CAM_Power

- `string PowerOn(int id) {}`
- `string PowerOff(int id) {}`
- `string PowerInquiry(int id) {}` (for this one, see CamPowerInq)

Example: `\x81\x01\x04\x00\x02\xFF` would be a Power On command for a camera at ID #1. Note that these are bytes with no spaces!

#### Overloaded Methods

Overloaded methods that do not need the speed variable and instead use the provided ones in the constructor.

#### HandleResponse

A feedback-parsing method called `HandleResponse` that will interpret acknowledgement, command execution, and power inquiry feedback. Use a boolean named `powerIsOn` to be set to true when the power inquiry returns true, and false when false. It will need to gather bytes together into a buffer up until it finds the first `\xFF` character and then interpret the response from there (and remove interpreted responses from the buffer). The buffer should also have some safety mechanism to clear itself if it gets "too big" (let's say > 100 chars) to avoid memory overruns.

Note the ID change in the responses also -- `y = x + 8`! So an `x = 1` would show `\x90\x50\x02\xFF` for example.

Implement `HandleResponse` using a separate `ResponseBuffer` class:

## `ResponseBuffer` Class

When a camera is given a command, it will send a response as a string. It may come all at once, or a few bytes at a time. In any case, it will end with `\xFF`, the VISCA delimiter.

We will use a string buffer variable to store the response fragments in. After every addition to the buffer, check whether the delimiter has been reached. When it has, extract the data up to and including the `\xFF` and return it as a string.

Some considerations:

- What if the triggers happen so fast that a second trigger happens while the first one is either writing to, or extracting from, the string buffer? Could we have a race condition or memory issue?
  - Use mutexes to lock the buffer while it is being written to or read from.
- What if data gets stuck in the buffer? What if I receive some garbage bytes before I get my useful bytes?
  - Check if adding the next command fragment would make it too long, and if so, clear the buffer.
- Sometimes, some commands will trigger two responses in series: \x90\x41\xFF\x90\x51\xFF is a common pair of responses to receive. How do you make sure you extract them both if they come together, or separately?
  - Iterate through the buffer and extract all responses, not just one.

## `EventArgs`, `DataEventArgs` Classes

During communication between devices via a physical port, data bytes will be streaming in both direfctions. The control program alerts us when data bytes are received, and what those data bytes are. The alert object from the hardware device running the control problem is an **event**, and the data received are the **`EventArgs`** (arguments). The `EventArgs` will contain as a field the received bytes. The bytes are then added to the `ResponseBuffer` until we get a complete message. Once we have a complete message, we will transmit it to other parts of the program for interpretation via our own event.

We will create a public event associated with the data buffer, which will be raised whenever we have a complete message. The event will need to have the completed message we gather as `EventArgs` so that we, and other interested parties, can interpret the message properly and report proper device status. As such, we will create an object called `DataEventArgs` the will inherit from the native EventArgs class that is part of the native `EventHandler<EventArgs>` construction and provide the entire data string as a field that listeners (subscribers, if you will) can interpret it however they like.

## `TransmitData` Class
