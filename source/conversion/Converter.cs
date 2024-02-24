




using System.ComponentModel;
using System.Diagnostics;

namespace unknown_horizons_spriteconversion;
class Converter
{
    private readonly string sourceDirectory;
    private readonly string targetDirectory;
    private readonly string tempDirectory;

    public Converter(string sourceDirectory, string targetDirectory)
    {
        this.sourceDirectory = sourceDirectory;
        this.targetDirectory = targetDirectory;
        this.tempDirectory = targetDirectory + "Temp";
    }

    public void ConvertImages()
    {
        //GenerateTempSpriteSheets();
        ClearTargetDirectory();
        List<string> dirs = new List<string>(Directory.EnumerateDirectories(this.tempDirectory));

        foreach (var tempCategoryDirectory in dirs)
        {
            //Console.WriteLine($"Processing: {dir}");
            string categoryName = FirstCharToUpper(tempCategoryDirectory.Substring(tempCategoryDirectory.LastIndexOf(Path.DirectorySeparatorChar) + 1));
            string targetCategoryDirectory = targetDirectory + Path.DirectorySeparatorChar + categoryName;
            if (!Directory.Exists(targetCategoryDirectory))
            {
                Directory.CreateDirectory(targetCategoryDirectory);
            }

            CopySpriteSheets(tempCategoryDirectory, targetCategoryDirectory);
            //Console.WriteLine($"Processing: {dir} END");
        }
    }

    private void GenerateTempSpriteSheets()
    {
        ClearTempDirectory();
        List<string> dirs = new List<string>(Directory.EnumerateDirectories(this.sourceDirectory));

        foreach (var categoryDir in dirs)
        {
            //Console.WriteLine($"Processing: {dir}");
            string categoryName = FirstCharToUpper(categoryDir.Substring(categoryDir.LastIndexOf(Path.DirectorySeparatorChar) + 1));
            string tempCategoryDirectory = tempDirectory + Path.DirectorySeparatorChar + categoryName;
            if (!Directory.Exists(tempCategoryDirectory))
            {
                Directory.CreateDirectory(tempCategoryDirectory);
            }

            GenerateSpriteSheets(categoryDir, tempCategoryDirectory);
            //Console.WriteLine($"Processing: {dir} END");
        }
    }

    private void CopySpriteSheets(string tempCategoryDirectory, string targetCategoryDirectory)
    {
        List<string> objectDirs = new List<string>(Directory.EnumerateDirectories(tempCategoryDirectory));
        foreach (var objectImageDir in objectDirs)
        {
            Console.WriteLine($"Processing: {objectImageDir}");

            string lastChar = objectImageDir.Substring(objectImageDir.Length - 1);
            string secondLastChar = objectImageDir.Substring(objectImageDir.Length - 2, 1);
            string toReplace = "";
            string replaceWith = "";

            if (int.TryParse(lastChar, out int index) && !secondLastChar.Equals("x"))
            {
                toReplace = index.ToString();
                if (index == 0)
                {
                    if (Directory.Exists(objectImageDir.Replace('0', '1')))
                    {
                        replaceWith = "1";
                    }
                }
                else
                {
                    replaceWith = (index + 1).ToString();
                }
            }
            //Console.WriteLine($"Processing: {objectImageDir}; lastChar: {lastChar}; secondLastChar: {secondLastChar}; toReplace:-{toReplace.Length}-;replaceWith:-{replaceWith}-");
            string objectName = objectImageDir.Substring(objectImageDir.LastIndexOf(Path.DirectorySeparatorChar) + 1);

            if (toReplace.Length > 0)
            {
                objectName = objectName.Replace(toReplace, replaceWith);
            }

            if (tempCategoryDirectory.Contains("Trees"))
            {
                objectName = "";//Tree hack
            }

            string targetObjectDirectory = targetCategoryDirectory + Path.DirectorySeparatorChar + objectName;
            if (!Directory.Exists(targetObjectDirectory))
            {
                Directory.CreateDirectory(targetObjectDirectory);
            }

            List<string> files = new List<string>(Directory.EnumerateFiles(objectImageDir));
            foreach (var imageFile in files)
            {
                string filename = imageFile.Substring(imageFile.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                string targetImageFile = filename;
                if (toReplace.Length > 0)
                {
                    targetImageFile = filename.Replace(toReplace, replaceWith);
                }
                //Console.WriteLine("Imagefile: " + imageFile);
                //Console.WriteLine("targetImageFile: " + targetObjectDirectory + Path.DirectorySeparatorChar + targetImageFile);
                File.Copy(imageFile, targetObjectDirectory + Path.DirectorySeparatorChar + targetImageFile);
            }

            Console.WriteLine($"Processing: {objectName} END");
        }
    }

    private void GenerateSpriteSheets(string sourceCategoryDir, string targetCategoryDirectory)
    {
        List<string> objectDirs = new List<string>(Directory.EnumerateDirectories(sourceCategoryDir));
        foreach (var objectImageDir in objectDirs)
        {
            string objectName = FirstCharToUpper(objectImageDir.Substring(objectImageDir.LastIndexOf(Path.DirectorySeparatorChar) + 1).Replace("as_", ""));
            Console.WriteLine($"Processing: {objectName}");
            string targetObjectDirectory = targetCategoryDirectory + Path.DirectorySeparatorChar + objectName;
            if (!Directory.Exists(targetObjectDirectory))
            {
                Directory.CreateDirectory(targetObjectDirectory);
            }
            List<string> imageTypeDirs = new List<string>(Directory.EnumerateDirectories(objectImageDir));
            foreach (var imageTypeDir in imageTypeDirs)
            {
                string imageType = imageTypeDir.Substring(imageTypeDir.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                Console.WriteLine($"Image type: {imageType}");
                string imageTypeDir45 = imageTypeDir + Path.DirectorySeparatorChar + "45";
                int spriteSheetRowCount = 2;
                int frameCount = Directory.GetFiles(imageTypeDir45).Length;
                if (frameCount > 1)
                {
                    List<string> dirs = new List<string>(Directory.EnumerateDirectories(imageTypeDir));
                    spriteSheetRowCount = dirs.Count;
                }
                CreateSprite(imageTypeDir, targetObjectDirectory, objectName, imageType, spriteSheetRowCount);
            }
            Console.WriteLine($"Processing: {objectName} END");
        }
    }

    private void CreateSprite(string imageTypeDir, string targetObjectDirectory, string objectName, string imageType, int spriteSheetRowCount)
    {
        List<string> files = new List<string>(Directory.EnumerateFiles(imageTypeDir, "*.png", SearchOption.AllDirectories));
        foreach (var imageFile in files)
        {
            Console.WriteLine($"Imagefile: {imageFile}");
            //Console.WriteLine(imageFile.Substring(imageTypeDir.Length + 1, imageFile.Length - imageTypeDir.Length - 5));
            string[] imageNameParts = imageFile.Substring(imageTypeDir.Length + 1, imageFile.Length - imageTypeDir.Length - 5).Split(Path.DirectorySeparatorChar);
            if (imageNameParts.Length == 2)
            {
                string spritePartTargetFilename = Path.DirectorySeparatorChar + "image_" + Int32.Parse(imageNameParts[0]).ToString("D3") + "_" + Int32.Parse(imageNameParts[1]).ToString("D3");
                string copyToFileName = imageTypeDir + spritePartTargetFilename + ".png";
                if (!File.Exists(copyToFileName))
                {
                    Console.WriteLine("Copy filename:" + copyToFileName);
                    File.Copy(imageFile, copyToFileName);
                }
            }
            /*
            else
            {
                Console.WriteLine($"NOT PROCESSED: {imageFile}");
            }*/
        }

        string tileArg = "x" + spriteSheetRowCount;
        string spritesheetPath = targetObjectDirectory + Path.DirectorySeparatorChar + objectName + "_" + imageType + ".png";
        //"montage -alpha on -background none -mode concatenate -tile " + tile_arg + " " + render_path + "*.png " + spritesheet_path
        string montageArgs = "-alpha on -background none -mode concatenate -tile " + tileArg + " " + imageTypeDir + Path.DirectorySeparatorChar + "*.png " + spritesheetPath;

        Console.WriteLine($"montageArgs: {montageArgs}");
        Process process = new Process();
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.FileName = "montage";
        process.StartInfo.Arguments = montageArgs;
        process.Start();
        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            Console.WriteLine(process.ExitCode);
            Environment.Exit(0);
        }

    }

    private string FirstCharToUpper(string dir)
    {
        return char.ToUpper(dir[0]) + dir.Substring(1);
    }

    private void ClearTargetDirectory()
    {
        if (!Directory.Exists(this.targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }
        else
        {
            Directory.Delete(targetDirectory, true);
            Directory.CreateDirectory(targetDirectory);
        }
    }

    private void ClearTempDirectory()
    {
        if (!Directory.Exists(this.targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }
        else
        {
            Directory.Delete(targetDirectory, true);
            Directory.CreateDirectory(targetDirectory);
        }
    }


}