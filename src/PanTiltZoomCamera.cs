public class PanTiltZoomCamera
{
    private int id;
    private int panSpeed;
    private int tiltSpeed;
    private int zoomSpeed;

    // private methods to validate fields
    private void ValidateId(int id)
    {
        if (id < 0x1 || id > 0xF)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "ID must be between 0x1 and 0xF inclusive.");
        }
    }

    private void ValidatePanSpeed(int panSpeed)
    {
        if (panSpeed < 0x01 || panSpeed > 0x18)
        {
            throw new ArgumentOutOfRangeException(nameof(panSpeed), "Pan speed must be between 0x01 and 0x18 inclusive.");
        }
    }

    private void ValidateTiltSpeed(int tiltSpeed)
    {
        if (tiltSpeed < 0x01 || tiltSpeed > 0x14)
        {
            throw new ArgumentOutOfRangeException(nameof(tiltSpeed), "Tilt speed must be between 0x01 and 0x14 inclusive.");
        }
    }

    private void ValidateZoomSpeed(int zoomSpeed)
    {
        if (zoomSpeed < 0x0 || zoomSpeed > 0x7)
        {
            throw new ArgumentOutOfRangeException(nameof(zoomSpeed), "Zoom speed must be between 0x0 and 0x7 inclusive.");
        }
    }

    private void VerifyPresetNumber(int presetNumber)
    {
        if (presetNumber < 0x0 || presetNumber > 0xF)
        {
            throw new ArgumentOutOfRangeException(nameof(presetNumber), "Preset number must be between 0x0 and 0xF inclusive.");
        }
    }

    // private method to validate and initialize fields
    private void Initialize(int id, int panSpeed, int tiltSpeed, int zoomSpeed)
    {
        ValidateId(id);
        ValidatePanSpeed(panSpeed);
        ValidateTiltSpeed(tiltSpeed);
        ValidateZoomSpeed(zoomSpeed);

        this.id = id;
        this.panSpeed = panSpeed;
        this.tiltSpeed = tiltSpeed;
        this.zoomSpeed = zoomSpeed;
    }

    // constructors
    public PanTiltZoomCamera(int id)
    {
        Initialize(id, 0x01, 0x01, 0x0);
    }

    public PanTiltZoomCamera()
    {
        Initialize(0x1, 0x01, 0x01, 0x0);
    }

    public PanTiltZoomCamera(int id, int panSpeed, int tiltSpeed, int zoomSpeed)
    {
        Initialize(id, panSpeed, tiltSpeed, zoomSpeed);
    }

    // public methods with validation
    // Pan_tiltDrive methods
    public string Up(int id, int panSpeed)
    {
        ValidateId(id);
        ValidatePanSpeed(panSpeed);
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x00\x03\x01\xFF";
    }

    public string Up()
    {
        return Up(this.id, this.panSpeed);
    }

    public string Down(int id, int panSpeed)
    {
        ValidateId(id);
        ValidatePanSpeed(panSpeed);
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x00\x03\x02\xFF";
    }

    public string Down()
    {
        return Down(this.id, this.panSpeed);
    }

    public string Left(int id, int panSpeed)
    {
        ValidateId(id);
        ValidatePanSpeed(panSpeed);
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x00\x01\x03\xFF";
    }

    public string Left()
    {
        return Left(this.id, this.panSpeed);
    }

    public string Right(int id, int panSpeed)
    {
        ValidateId(id);
        ValidatePanSpeed(panSpeed);
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x00\x02\x03\xFF";
    }

    public string Right()
    {
        return Right(this.id, this.panSpeed);
    }

    public string UpLeft(int id, int panSpeed, int tiltSpeed)
    {
        ValidateId(id);
        ValidatePanSpeed(panSpeed);
        ValidateTiltSpeed(tiltSpeed);
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x{tiltSpeed:X2}\x01\x01\xFF";
    }

    public string UpLeft()
    {
        return UpLeft(this.id, this.panSpeed, this.tiltSpeed);
    }

    public string UpRight(int id, int panSpeed, int tiltSpeed)
    {
        ValidateId(id);
        ValidatePanSpeed(panSpeed);
        ValidateTiltSpeed(tiltSpeed);
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x{tiltSpeed:X2}\x02\x01\xFF";
    }

    public string UpRight()
    {
        return UpRight(this.id, this.panSpeed, this.tiltSpeed);
    }

    public string DownLeft(int id, int panSpeed, int tiltSpeed)
    {
        ValidateId(id);
        ValidatePanSpeed(panSpeed);
        ValidateTiltSpeed(tiltSpeed);
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x{tiltSpeed:X2}\x01\x02\xFF";
    }

    public string DownLeft()
    {
        return DownLeft(this.id, this.panSpeed, this.tiltSpeed);
    }

    public string DownRight(int id, int panSpeed, int tiltSpeed)
    {
        ValidateId(id);
        ValidatePanSpeed(panSpeed);
        ValidateTiltSpeed(tiltSpeed);
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x{tiltSpeed:X2}\x02\x02\xFF";
    }

    public string DownRight()
    {
        return DownRight(this.id, this.panSpeed, this.tiltSpeed);
    }

    public string StopPanTilt(int id)
    {
        ValidateId(id);
        return $"\x8{id:X1}\x01\x06\x01\x00\x00\x03\x03\xFF";
    }

    public string StopPanTilt()
    {
        return StopPanTilt(this.id);
    }

    // CAM_Zoom methods
    public string ZoomIn(int id, int zoomSpeed)
    {
        ValidateId(id);
        ValidateZoomSpeed(zoomSpeed);
        return $"\x8{id:X1}\x01\x04\x07\x2{zoomSpeed:X1}\xFF";
    }

    public string ZoomIn()
    {
        return ZoomIn(this.id, this.zoomSpeed);
    }

    public string ZoomOut(int id, int zoomSpeed)
    {
        ValidateId(id);
        ValidateZoomSpeed(zoomSpeed);
        return $"\x8{id:X1}\x01\x04\x07\x3{zoomSpeed:X1}\xFF";
    }

    public string ZoomOut()
    {
        return ZoomOut(this.id, this.zoomSpeed);
    }

    public string StopZoom(int id)
    {
        ValidateId(id);
        return $"\x8{id:X1}\x01\x04\x07\x00\xFF";
    }

    public string StopZoom()
    {
        return StopZoom(this.id);
    }

    // CAM_Memory methods
    public string StorePreset(int id, int presetNumber)
    {
        ValidateId(id);
        VerifyPresetNumber(presetNumber);
        return $"\x8{id:X1}\x01\x04\x3F\x01\x0{presetNumber:X1}\xFF";
    }

    public string StorePreset(int presetNumber)
    {
        return StorePreset(this.id, presetNumber);
    }

    public string RecallPreset(int id, int presetNumber)
    {
        ValidateId(id);
        VerifyPresetNumber(presetNumber);
        return $"\x8{id:X1}\x01\x04\x3F\x02\x0{presetNumber:X1}\xFF";
    }

    public string RecallPreset(int presetNumber)
    {
        return RecallPreset(this.id, presetNumber);
    }

    // CAM_Power methods
    public string PowerOn(int id)
    {
        ValidateId(id);
        return $"\x8{id:X1}\x01\x04\x00\x02\xFF";
    }

    public string PowerOn()
    {
        return PowerOn(this.id);
    }

    public string PowerOff(int id)
    {
        ValidateId(id);
        return $"\x8{id:X1}\x01\x04\x00\x03\xFF";
    }

    public string PowerOff()
    {
        return PowerOff(this.id);
    }

    public string PowerInquiry(int id)
    {
        ValidateId(id)
        // TODO
        return "TODO";
    }

    public string PowerInquiry()
    {
        return PowerInquiry(this.id);
    }

    public string HandleResponse()
    {
        // TODO
        return "TODO";
    }
}
