using CommandLine;
using System.Collections.Immutable;

namespace mini_pictures
{
    public class MiniPictureCommandLineOptions
    {
        private string _purpose = "undefined";

        [Option('m', "minside", Required = false, HelpText = "Mini-Picture minimum side length. Default value: 3")]
        public int MinSide { get; set; }

        [Option('M', "maxside", Required = false, HelpText = "Mini-Picture maximum side length. Default value: 9")]
        public int MaxSide { get; set; }

        [Option('q', "images-number", Required = true, HelpText = "Total number of pictures (quantity) to generate of each shape.")]
        public int MaxPictures { get; set; }

        [Option('b', "batchsize", Required = false, HelpText = "Max number of generated pictures to keep in memory before splitting results in multiple sequence files. " +
            "Default value: 50,000")]
        public int BatchSize { get; set; }

        [Option('n', "experiment_name", Required = true, HelpText = "The name for this generated set of images.")]
        public string ExprerimentName { get; set; }

        [Option('F', "base_folder", Required = false, HelpText = "The base folder to save the experiment data, and images (if requested). Default value: 'C:\\mini-pictures'")]
        public string BaseFolderName { get; set; }

        [Option('P', "data_purpose", Required = false, HelpText = "The purpose of the data; one of the following values: Train, Test, Generic. Default value: Train")]
        public string Purpose 
        {
            get { return _purpose; }
            set 
            {
                if (string.IsNullOrEmpty(value) || ! DataPurposeExtensions.IsDataPurpose(value))
                    throw new ArgumentOutOfRangeException(nameof(value),
                      $"Valid values are: [{string.Join(", ", DataPurposeExtensions.DataPurposes)}]");
                _purpose = value;
            }
        }

        [Option('s', "save_images", Required = false, HelpText = "Save generated images to disk as JPEG. Default value: false")]
        public bool SaveImagesToDisk { get; set; }

        [Option('T', "train-data", Required = false, HelpText = "Generate trainning data. Default value: false")]
        public bool TrainData { get; set; }

        [Option('t', "test-data", Required = false, HelpText = "Generate testing data. Default value: false")]
        public bool TestData { get; set; }

        [Option('G', "generic-data", Required = false, HelpText = "Generate any purpose data. Default value: false")]
        public bool GenericData { get; set; }

        public MiniPictureCommandLineOptions()
        {
            MinSide = 3;
            MaxSide = 9;
            BatchSize = 50000;
            ExprerimentName = "mini-pictures-v1";
            BaseFolderName = "C:\\mini-pictures";
            SaveImagesToDisk = false;
            TrainData = false;
            TestData = false;
            GenericData = false;
        }
    }
}
