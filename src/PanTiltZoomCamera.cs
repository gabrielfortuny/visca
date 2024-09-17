public class PanTiltZoomCamera
{
    private int id;
    private int panSpeed;
    private int tiltSpeed;
    private int zoomSpeed;

    // Private method to validate and initialize fields
    private void InitializeCamera(int id, int panSpeed, int tiltSpeed, int zoomSpeed)
    {
        if (id < 0x1 || id > 0xF)
        {
            throw new ArgumentOutOfRangeException(nameof(id), "ID must be between 0x1 and 0xF inclusive.");
        }

        if (panSpeed < 0x01 || panSpeed > 0x18)
        {
            throw new ArgumentOutOfRangeException(nameof(panSpeed), "Pan speed must be between 0x01 and 0x18 inclusive.");
        }

        if (tiltSpeed < 0x01 || tiltSpeed > 0x14)
        {
            throw new ArgumentOutOfRangeException(nameof(tiltSpeed), "Tilt speed must be between 0x01 and 0x14 inclusive.");
        }

        if (zoomSpeed < 0x0 || zoomSpeed > 0x7)
        {
            throw new ArgumentOutOfRangeException(nameof(zoomSpeed), "Zoom speed must be between 0x0 and 0x7 inclusive.");
        }

        this.id = id;
        this.panSpeed = panSpeed;
        this.tiltSpeed = tiltSpeed;
        this.zoomSpeed = zoomSpeed;
    }

    private void VerifyPresetNumber(int presetNumber)
    {
        if (presetNumber < 0 || presetNumber > 9)
        {
            throw new ArgumentOutOfRangeException(nameof(presetNumber), "Preset number must be between 0 and 9 inclusive.");
        }
    }

    // Constructors
    public PanTiltZoomCamera(int id)
    {
        InitializeCamera(id, 0x01, 0x01, 0x0);
    }

    public PanTiltZoomCamera()
    {
        InitializeCamera(0x1, 0x01, 0x01, 0x0);
    }

    public PanTiltZoomCamera(int id, int panSpeed, int tiltSpeed, int zoomSpeed)
    {
        InitializeCamera(id, panSpeed, tiltSpeed, zoomSpeed);
    }

    // Pan_tiltDrive methods
    public string Up(int id, int panSpeed)
    {
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x00\x03\x01\xFF";
    }

    public string Up()
    {
        return Up(this.id, this.panSpeed);
    }

    public string Down(int id, int panSpeed)
    {
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x00\x03\x02\xFF";
    }

    public string Down()
    {
        return Down(this.id, this.panSpeed);
    }

    public string Left(int id, int panSpeed)
    {
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x00\x01\x03\xFF";
    }

    public string Left()
    {
        return Left(this.id, this.panSpeed);
    }

    public string Right(int id, int panSpeed)
    {
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x00\x02\x03\xFF";
    }

    public string Right()
    {
        return Right(this.id, this.panSpeed);
    }

    public string UpLeft(int id, int panSpeed, int tiltSpeed)
    {
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x{tiltSpeed:X2}\x01\x01\xFF";
    }

    public string UpLeft()
    {
        return UpLeft(this.id, this.panSpeed, this.tiltSpeed);
    }

    public string UpRight(int id, int panSpeed, int tiltSpeed)
    {
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x{tiltSpeed:X2}\x02\x01\xFF";
    }

    public string UpRight()
    {
        return UpRight(this.id, this.panSpeed, this.tiltSpeed);
    }

    public string DownLeft(int id, int panSpeed, int tiltSpeed)
    {
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x{tiltSpeed:X2}\x01\x02\xFF";
    }

    public string DownLeft()
    {
        return DownLeft(this.id, this.panSpeed, this.tiltSpeed);
    }

    public string DownRight(int id, int panSpeed, int tiltSpeed)
    {
        return $"\x8{id:X1}\x01\x06\x01\x{panSpeed:X2}\x{tiltSpeed:X2}\x02\x02\xFF";
    }

    public string DownRight()
    {
        return DownRight(this.id, this.panSpeed, this.tiltSpeed);
    }

    public string StopPanTilt(int id)
    {
        return $"\x8{id:X1}\x01\x06\x01\x00\x00\x03\x03\xFF";
    }

    public string StopPanTilt()
    {
        return StopPanTilt(this.id);
    }

    // CAM_Zoom methods
    public string ZoomIn(int id, int zoomSpeed)
    {
        return $"\x8{id:X1}\x01\x04\x07\x2{zoomSpeed:X1}\xFF";
    }

    public string ZoomIn()
    {
        return ZoomIn(this.id, this.zoomSpeed);
    }

    public string ZoomOut(int id, int zoomSpeed)
    {
        return $"\x8{id:X1}\x01\x04\x07\x3{zoomSpeed:X1}\xFF";
    }

    public string ZoomOut()
    {
        return ZoomOut(this.id, this.zoomSpeed);
    }

    public string StopZoom(int id)
    {
        return $"\x8{id:X1}\x01\x04\x07\x00\xFF";
    }

    public string StopZoom()
    {
        return StopZoom(this.id);
    }

    // CAM_Memory methods
    public string StorePreset(int id, int presetNumber)
    {
        VerifyPresetNumber(presetNumber);
        return $"\x8{id:X1}\x01\x04\x3F\x01\x0{presetNumber:X1}\xFF";
    }

    public string StorePreset(int presetNumber)
    {
        return StorePreset(this.id, presetNumber);
    }


    public string RecallPreset(int id, int presetNumber)
    {
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
        return $"\x8{id:X1}\x01\x04\x00\x02\xFF";
    }

    public string PowerOn()
    {
        return PowerOn(this.id);
    }

    public string PowerOff(int id)
    {
        return $"\x8{id:X1}\x01\x04\x00\x03\xFF";
    }

    public string PowerOff()
    {
        return PowerOff(this.id);
    }

    public string PowerInquiry(int id)
    {
        int y = id + 8;

        // TODO
        return "";
    }

    public string PowerInquiry()
    {
        return PowerInquiry(this.id);
    }

    public string HandleResponse()
    {
        // TODO
        return "";
    }

}