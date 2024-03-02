namespace unknown_horizons_spriteconversion;
class ImageData
{

    public string CategoryName { get; private set; }
    public string ObjectName { get; set; }
    public string ImageType { get; private set; }
    public int Rows { get; private set; }
    public int Columns { get; private set; }
    public int SpriteWidth { get; set; }
    public int SpriteHeight { get; set; }

    public ImageData(string categoryName, string objectName, string imageType, int rows, int columns)
    {
        CategoryName = categoryName;
        ObjectName = objectName;
        ImageType = imageType;
        Rows = rows;
        Columns = columns;
        SpriteWidth = 0;
        SpriteHeight = 0;
    }

    public override string ToString()
    {
        return $"CN:{CategoryName};ON={ObjectName};IT:{ImageType};R:{Rows};C:{Columns};SW:{SpriteWidth};SH:{SpriteHeight}";
    }

    public string ToString(string newFileName)
    {
        char sepChar = Path.DirectorySeparatorChar;
        return $"{CategoryName}{sepChar}{ObjectName}{sepChar}{newFileName};{Rows};{Columns};{SpriteWidth};{SpriteHeight}";
    }
}