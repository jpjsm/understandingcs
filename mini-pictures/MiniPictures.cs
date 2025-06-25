using CommandLine;
using SkiaSharp;
using System.Drawing;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;

namespace mini_pictures
{
    internal class MiniPictures
    {

        private static readonly Random rand = new Random(DateTime.Now.Millisecond);
        private static readonly RandomNormal randgauss = new RandomNormal();
        private static readonly Dictionary<char, (string labelName, double[] labelOutput)> label_to_name = new() {
            {'-', ("dash", [0.98, 0.02, 0.02, 0.02]) },
            {'\\', ("backslash", [0.02, 0.98, 0.02, 0.02]) },
            {'|', ("pipe", [0.02, 0.02, 0.98, 0.02]) },
            {'/', ("slash", [0.02, 0.02, 0.02, 0.98]) },
        };
        private static readonly char[] labels = label_to_name.Keys.OrderBy(k => k).ToArray();
        private const int MagicNumberToEstimateMaxJsonString = 152100000; // By experimenting MaxPictures * Labels * Side * Side -> 25,000 * 4 * 39 * 39

        static int Main(string[] args)
        {

            int maxside = 32;
            int minside = 3;
            int MaxPictures = 25000;
            int batchsize = 50000;
            bool save_images_to_disk = false;
            string experiment_name = "mini-pictures-v1";
            string baseFolderName = "C:\\mini-$pictures";
            DataPurpose dataPurpose = DataPurpose.Train;
            bool argumentErrors = false;
            Parser.Default.ParseArguments<MiniPictureCommandLineOptions>(args)
                .WithParsed(options =>
                {
                    maxside = options.MaxSide;
                    minside = options.MinSide;
                    MaxPictures = options.MaxPictures;
                    batchsize = options.BatchSize;
                    save_images_to_disk = options.SaveImagesToDisk;
                    experiment_name = options.ExprerimentName;
                    baseFolderName = options.BaseFolderName;

                    if (options.GenericData)
                    {
                        dataPurpose = DataPurpose.Generic;
                    }

                    if (options.TestData)
                    {
                        dataPurpose = DataPurpose.Test;
                    }

                    if (options.TrainData)
                    {
                        dataPurpose = DataPurpose.Train;
                    }

                    if (DataPurposeExtensions.IsDataPurpose(options.Purpose))
                    {
                        dataPurpose = (DataPurpose) Enum.Parse(typeof(DataPurpose), options.Purpose);
                    }
                })
                .WithNotParsed(errors =>
                {
                    argumentErrors = true;
                });

            if (argumentErrors)
                return -1;

            Console.WriteLine("Hello, Mini-Pictures!");
            int targetbatchsize = batchsize;
            while (targetbatchsize > 0 && targetbatchsize * labels.Length * maxside * maxside > MagicNumberToEstimateMaxJsonString)
                targetbatchsize--;

            if (targetbatchsize <= 0)
            {
                Console.WriteLine("ERROR: Max-side too big!!");
                return -1;
            }

            if (targetbatchsize != batchsize && targetbatchsize < MaxPictures)
            {
                var originalBackgrondColor = Console.BackgroundColor;
                var originalForeGroundColor = Console.ForegroundColor;
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Warning: Batchsize, {batchsize}, adjusted to new size, {targetbatchsize}, to avoid 'OutOfMemory' exception.");
                batchsize = targetbatchsize;
                Console.BackgroundColor = originalBackgrondColor;
                Console.ForegroundColor = originalForeGroundColor;
            }

            int totalImages = MaxPictures * labels.Length;
            int json_str_size = 1024 * 18 * totalImages;

            try
            {
                for (int side = minside; side <= maxside; side++)
                {
                    Console.WriteLine($"Generating {side}x{side} images...");
                    List<minipicture_data_element> minipictures = new List<minipicture_data_element>(totalImages);
                    int subset = 1;
                    for (int sequence = 1; sequence <= MaxPictures; sequence++)
                    {
                        foreach (char label in labels)
                        {
                            using (SKBitmap bmp = new(side, side))
                            {
                                using (SKCanvas canvas = new(bmp))
                                {
                                    canvas.Clear(SKColors.White);

                                    minipictures.Add(DrawMiniPic(label, side, bmp, canvas));

                                    // Save the image to disk
                                    if (save_images_to_disk)
                                        SaveImageToDisk(baseFolderName, experiment_name, side, label, sequence, bmp);
                                }
                            }
                        }

                        if (sequence % batchsize == 0)
                            SaveMiniPictures(minipictures, baseFolderName, experiment_name, dataPurpose, totalImages, side, ref subset);
                    }

                    if (minipictures.Count > 0)
                        SaveMiniPictures(minipictures, baseFolderName, experiment_name, dataPurpose, totalImages, side, ref subset);

                    Console.WriteLine($"Finished {side}x{side} images...");

                    Console.WriteLine();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}");
                Console.WriteLine($"Exception Source: {ex.Source}");
                Console.WriteLine($"Exception StackTrace: {ex.StackTrace}");
                return -2;
            }

            Console.WriteLine("Goodbye, Mini-Pictures!");
            return 0;
        }

        private static minipicture_data_element DrawMiniPic(char label, int side, SKBitmap bmp, SKCanvas canvas)
        {
            double[] gray_minipicture_pixels;
            double[] gray_minipicture_pixels_norm;

            canvas.Clear(SKColors.White);
            int darkness = (int)(96.0 * randgauss.NextDouble());
            string gray_rgb = $"#{darkness:D2}{darkness:D2}{darkness:D2}";
            SKPaint paint = new() { Color = SKColor.Parse(gray_rgb) };
            paint.StrokeWidth = 1 + (int)(0.07 * rand.NextDouble() * side);

            // Define line endpoints based on label
            SKPoint pt1 = SKPoint.Empty;
            SKPoint pt2 = SKPoint.Empty;

            // side == left + center + right
            // after dividing side by 3, there might by some reminder
            // that reminder must be accounted for and distributed among left, center, and right.
            // if reminder is 1 add it to center
            // if reminder is 2 add 1 to left and 1 to right
            int left, center, right;
            left = center = right = side / 3;
            int mod3 = side % 3;
            switch (mod3)
            {
                case 1:
                    center++;
                    break;
                case 2:
                    left++;
                    right++;
                    break;
                default:
                    // this is case zero: do nothing
                    break;
            }

            float x0, y0, x1, y1;
            int delta;

            switch (label)
            {
                case '-':
                    x0 = 0;
                    y0 = left + (float)(Convert.ToDouble(center) * randgauss.NextDouble());
                    pt1 = new SKPoint(x0, y0);

                    x1 = side;
                    y1 = left + (float)(Convert.ToDouble(center) * randgauss.NextDouble());
                    pt2 = new SKPoint(x1, y1);
                    break;

                case '\\':
                    // Upper left starting point
                    // default value when delta == 0
                    x0 = 0;
                    y0 = 0;

                    delta = randgauss.Next((left << 1) - 1) - (left - 1);
                    if (delta < 0)
                        y0 -= delta;
                    if (delta > 0)
                        x0 += delta;

                    pt1 = new SKPoint(x0, y0);

                    // Lower right ending point
                    // default value when delta == 0
                    x1 = side;
                    y1 = side;

                    delta = randgauss.Next((left << 1) - 1) - (left - 1);
                    if (delta < 0)
                        x1 += delta;
                    if (delta > 0)
                        y1 -= delta;

                    pt2 = new SKPoint(x1, y1);
                    break;

                case '|':
                    pt1 = new SKPoint(left + (int)(Convert.ToDouble(center) * randgauss.NextDouble()), 0);
                    pt2 = new SKPoint(left + (int)(Convert.ToDouble(center) * randgauss.NextDouble()), side);
                    break;

                case '/':
                    // lower left starting point
                    // default value when delta == 0
                    x0 = 0;
                    y0 = side;

                    delta = randgauss.Next((left << 1) - 1) - (left - 1);
                    if (delta < 0)
                        y0 += delta;
                    if (delta > 0)
                        x0 += delta;

                    pt1 = new SKPoint(x0, y0);

                    // upper right ending point
                    // default value when delta == 0
                    x1 = side;
                    y1 = 0;

                    delta = randgauss.Next((left << 1) - 1) - (left - 1);
                    if (delta < 0)
                        x1 += delta;
                    if (delta > 0)
                        y1 += delta;

                    pt2 = new SKPoint(x1, y1);
                    break;

                default:
                    throw new ArgumentException($"Undefined label '{label}'.");
            }

            canvas.DrawLine(pt1, pt2, paint);

            // Get the bytes
            gray_minipicture_pixels = bmp.Bytes.Where((x, i) => i % 4 == 0).Select(b => Convert.ToDouble(b)).ToArray();
            gray_minipicture_pixels_norm = gray_minipicture_pixels.Select(x => x / 255.0).ToArray();

            return new minipicture_data_element(label, gray_minipicture_pixels, gray_minipicture_pixels_norm, side, side);
        }

        private static void SaveMiniPictures(
            List<minipicture_data_element> minipictures, 
            string baseFolderName, 
            string experiment_name,
            DataPurpose purpose,
            int totalImages, 
            int side, 
            ref int subset)
        {
            string experiment_folder = Path.Join(baseFolderName, experiment_name, purpose.GetName(),$"count_{totalImages}", $"{side}x{side}");
            Directory.CreateDirectory(experiment_folder);
            string minipictures_filename = Path.Join(experiment_folder, $"{experiment_name}-{purpose.GetName()}-{side}x{side}-{totalImages}-subset_{subset:D6}.json");
            File.WriteAllText(minipictures_filename, JsonSerializer.Serialize(minipictures));
            minipictures.Clear();
            Console.WriteLine($"saved: {minipictures_filename}");
            subset++;
        }
        
        private static void SaveImageToDisk(string baseFolderName, string experiment_name, int side, char label, int sequence, SKBitmap bmp)
        {
            string imagesBaseFolder = Path.Join(baseFolderName, experiment_name,  "images", $"{side}x{side}");
            Directory.CreateDirectory(imagesBaseFolder);

            string image_filename = Path.Join(imagesBaseFolder, $"{experiment_name}-{side}x{side}.{label_to_name[label].labelName}.{sequence:d6}.jpg");

            using (SKFileWStream fs = new(image_filename))
            {
                bmp.Encode(fs, SKEncodedImageFormat.Jpeg, quality: 100);
                fs.Flush();
            }

            Thread.Sleep(50);
        }

        public readonly record struct minipicture_data_element
        {
            public char Label { get; }
            public string LabelName { get; }
            public double[] Inputs { get; }
            public double[] InputsNormalized { get; }
            public double[] ExpectedOutputs { get; }
            public int Height { get; }
            public int Width { get; }

            public minipicture_data_element(char label, double[] inputs, double[] normalizedInputs, int height, int width)
            {
                Label = label;
                Inputs = inputs;
                InputsNormalized = normalizedInputs;
                LabelName = label_to_name[label].labelName;
                ExpectedOutputs = label_to_name[label].labelOutput;
                Height = height;
                Width = width;
            }

        }
    }
}
