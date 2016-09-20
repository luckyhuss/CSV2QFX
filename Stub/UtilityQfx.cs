using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSV2QFX.Stub
{
    /// <summary>
    /// Utility class for the project
    /// </summary>
    /// <author>Anwar Buchoo</author>
    /// <date>05/04/2013</date>
    public sealed class UtilityQfx
    {
        // Lazy initialisation of Singleton ( .NET 4.0+)
        private static readonly Lazy<UtilityQfx> Instance = new Lazy<UtilityQfx>(() => new UtilityQfx());
        
        /// <summary>
        /// DigitSeparator = ,
        /// </summary>
        public const char DigitSeparator = ',';

        /// <summary>
        /// ManualCsvSeparator = ;
        /// </summary>
        public const char ManualCsvSeparator = ';';

        /// <summary>
        /// CsvNewLine = \n
        /// </summary>
        public const char CsvNewLine = '\n';

        /// <summary>
        /// CsvSeparator = ,
        /// </summary>
        public const char CsvSeparator = ',';

        /// <summary>
        /// Gets and sets the manual input content
        /// </summary>
        public string ManualInputContent { get; set; }

        /// <summary>
        /// Gets the single instance of the class Utility
        /// </summary>
        public static UtilityQfx GetInstance
        {
            get { return Instance.Value; }
        }

        /// <summary>
        /// Opens the Chrome browser ( Selenium 2)
        /// </summary>
        public void OpenChromeDriver()
        {
            
        }
    }
}
