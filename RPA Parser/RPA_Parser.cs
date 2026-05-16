using Ionic.Zlib;
using Razorvine.Pickle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RPA_Parser
{
    // Inspired by: https://github.com/Shizmob/rpatool

    public class RpaParser
    {
        public class Version
        {
            public const double Unknown = -1;
            public const double RPA_1 = 1;
            public const double RPA_2 = 2;
            public const double RPA_3 = 3;
            public const double RPA_3_2 = 3.2;
        }

        private class ArchiveMagic
        {
            public const string RPA_1_RPA = ".rpa";
            public const string RPA_1_RPI = ".rpi";
            public const string RPA_2 = "RPA-2.0 ";
            public const string RPA_3 = "RPA-3.0 ";
            public const string RPA_3_2 = "RPA-3.2 ";
        }

        public FileInfo ArchiveInfo;
        public FileInfo IndexInfo;
        public double ArchiveVersion = Version.Unknown;
        public int Padding = 0;
        public long ObfuscationKey = 0xDEADBEEF;
        public bool OptionsConfirmed = false;
        public SortedDictionary<string, ArchiveIndex> Index = new SortedDictionary<string, ArchiveIndex>();

        private long _offset;
        private string _archivePath;
        private string _indexPath;
        private string _firstLine;
        private string[] _metadata;

        public class Tuples
        {
            public long Offset;
            public long Length;
            public byte[] Prefix;
        }

        public class ArchiveIndex
        {
            public readonly SortedDictionary<int, Tuples> Tuples = new SortedDictionary<int, Tuples>();
            public string FullPath = String.Empty;
            public string TreePath = String.Empty;
            public string ParentPath = String.Empty;
            public bool InArchive;
            public long Length;
        }

        public void LoadArchive(string filePath)
        {
            _archivePath = filePath;
            GetIndexAndArchive();
            ArchiveInfo = GetArchiveInfo();
            _firstLine = GetFirstLine();
            ArchiveVersion = CheckSupportedVersion(GetVersion());

            if (CheckVersion(ArchiveVersion, Version.RPA_2) || CheckVersion(ArchiveVersion, Version.RPA_3) || CheckVersion(ArchiveVersion, Version.RPA_3_2))
            {
                _metadata = GetMetadata();
                _offset = GetOffset();
                ObfuscationKey = GetObfuscationKey();
            }
            else if (CheckVersion(ArchiveVersion, Version.RPA_1))
            {
                IndexInfo = GetIndexInfo();
            }

            Index = GetIndexes();
        }

        public bool CheckVersion(double version, double check)
        {
            double difference = version - check;
            if (difference == 0)
            {
                return true;
            }

            return false;
        }

        private void GetIndexAndArchive()
        {
            if (_archivePath.ToLower().EndsWith(ArchiveMagic.RPA_1_RPA))
            {
                _indexPath = Regex.Replace(_archivePath, @"\.rpa$", ".rpi", RegexOptions.IgnoreCase);
            }
            if (_archivePath.ToLower().EndsWith(ArchiveMagic.RPA_1_RPI))
            {
                _indexPath = _archivePath;
                _archivePath = Regex.Replace(_archivePath, @"\.rpi$", ".rpa", RegexOptions.IgnoreCase);
            }
        }

        public double CheckSupportedVersion(double version)
        {
            switch (version)
            {
                case Version.RPA_3_2:
                case Version.RPA_3:
                case Version.RPA_2:
                case Version.RPA_1:
                    // Version is OK
                    break;
                default:
                    throw new Exception("Specified version is not supported.");
            }

            return version;
        }

        private FileInfo GetArchiveInfo()
        {
            if (_archivePath == String.Empty)
            {
                throw new Exception("No archive file provided.");
            }

            if (!File.Exists(_archivePath))
            {
                throw new Exception("Archive file does not exist.");
            }

            return new FileInfo(_archivePath);
        }

        private FileInfo GetIndexInfo()
        {
            if (_indexPath == String.Empty)
            {
                throw new Exception("No index file provided.");
            }

            if (!File.Exists(_indexPath))
            {
                throw new Exception("Index file does not exist.");
            }

            return new FileInfo(_indexPath);
        }

        private string GetFirstLine()
        {
            using (StreamReader streamReader = new StreamReader(_archivePath, Encoding.UTF8))
            {
                return streamReader.ReadLine();
            }
        }

        private double GetVersion()
        {
            if (_firstLine.StartsWith(ArchiveMagic.RPA_3_2))
            {
                return 3.2;
            }

            if (_firstLine.StartsWith(ArchiveMagic.RPA_3))
            {
                return 3;
            }

            if (_firstLine.StartsWith(ArchiveMagic.RPA_2))
            {
                return 2;
            }

            if (_archivePath.ToLower().EndsWith(ArchiveMagic.RPA_1_RPA) || _archivePath.ToLower().EndsWith(ArchiveMagic.RPA_1_RPI))
            {
                GetIndexAndArchive();
                if (File.Exists(_archivePath) && File.Exists(_indexPath))
                {
                    return 1;
                }
            }

            throw new Exception("File is either not valid RenPy Archive or version is not recognized.");
        }

        private string[] GetMetadata()
        {
            return _firstLine.Split(' ');
        }

        private long GetOffset()
        {
            return Convert.ToInt64(_metadata[1], 16);
        }

        private long GetObfuscationKey()
        {
            long key = 0;

            if (CheckVersion(ArchiveVersion, Version.RPA_3))
            {
                for (int i = 2; i < _metadata.Length; i++)
                {
                    key ^= Convert.ToInt64(_metadata[i], 16);
                }
            }
            else if (CheckVersion(ArchiveVersion, Version.RPA_3_2))
            {
                for (int i = 3; i < _metadata.Length; i++)
                {
                    key ^= Convert.ToInt64(_metadata[i], 16);
                }
            }

            return key;
        }

        private SortedDictionary<string, ArchiveIndex> GetIndexes()
        {
            SortedDictionary<string, ArchiveIndex> indexList = new SortedDictionary<string, ArchiveIndex>();
            object unpickledIndexes;

            string filePath = _archivePath;
            if (CheckVersion(ArchiveVersion, Version.RPA_1))
            {
                filePath = _indexPath;
            }

            using (BinaryReader reader = new BinaryReader(File.OpenRead(filePath), Encoding.UTF8))
            {
                if (CheckVersion(ArchiveVersion, Version.RPA_2) || CheckVersion(ArchiveVersion, Version.RPA_3) || CheckVersion(ArchiveVersion, Version.RPA_3_2))
                {
                    reader.BaseStream.Seek(_offset, SeekOrigin.Begin);
                }

                long blockOffset = _offset;
                long blockSize = 2046;
                long payloadSize = reader.BaseStream.Length;
                byte[] fileCompressed = { };

                while (blockSize > 0)
                {
                    //long remaining = payloadSize - blockOffset;
                    if (blockOffset + blockSize > payloadSize)
                    {
                        blockSize = payloadSize - blockOffset;

                        if (blockSize < 0)
                        {
                            blockSize = 0;
                        }
                    }

                    if (blockSize != 0)
                    {
                        byte[] buffer = reader.ReadBytes((int)blockSize);
                        fileCompressed = fileCompressed.Concat(buffer).ToArray();

                        blockOffset += blockSize;
                        reader.BaseStream.Seek(blockOffset, SeekOrigin.Begin);
                    }
                }

                byte[] fileUncompressed = ZlibStream.UncompressBuffer(fileCompressed);
                using (Unpickler unpickler = new Unpickler())
                {
                    unpickledIndexes = unpickler.loads(fileUncompressed);
                }
            }

            // Standardize output
            foreach (DictionaryEntry kvp in (Hashtable)unpickledIndexes)
            {
                if (kvp.Value == null)
                {
                    continue;
                }

                ArchiveIndex indexEntry = new ArchiveIndex
                {
                    TreePath = (string)kvp.Key,
                    ParentPath = Path.GetDirectoryName((string)kvp.Key),
                    InArchive = true
                };
                int counter = 0;
                foreach (object[] value in (ArrayList)kvp.Value)
                {
                    Tuples index = new Tuples
                    {
                        Offset = Convert.ToInt64(value.GetValue(0)),
                        Length = Convert.ToInt64(value.GetValue(1))
                    };
                    if ((long)value.Length == 3)
                    {
                        if (value.GetValue(2).GetType() == typeof(byte[]))
                        {
                            index.Prefix = (byte[])value.GetValue(2);
                        }
                        else
                        {
                            index.Prefix = Encoding.UTF8.GetBytes((string)value.GetValue(2));
                        }
                    }
                    else
                    {
                        index.Prefix = Array.Empty<byte>();
                    }

                    indexEntry.Tuples.Add(counter, index);
                    counter++;
                }
                indexList.Add(indexEntry.TreePath, indexEntry);
            }

            foreach (KeyValuePair<string, ArchiveIndex> kvp in indexList)
            {
                foreach (KeyValuePair<int, Tuples> kvpI in kvp.Value.Tuples)
                {
                    // Deobfuscate index data
                    if (ArchiveVersion >= Version.RPA_3)
                    {
                        kvpI.Value.Offset ^= ObfuscationKey;
                        kvpI.Value.Length ^= ObfuscationKey;
                    }

                    kvp.Value.Length += kvpI.Value.Length;
                }
            }

            return indexList;
        }

        public SortedDictionary<string, ArchiveIndex> DeepCopyIndex(SortedDictionary<string, ArchiveIndex> originalIndex)
        {
            SortedDictionary<string, ArchiveIndex> indexCopy = new SortedDictionary<string, ArchiveIndex>();

            foreach (KeyValuePair<string, ArchiveIndex> kvp in originalIndex)
            {
                ArchiveIndex archIndex = new ArchiveIndex
                {
                    FullPath = kvp.Value.FullPath,
                    InArchive = kvp.Value.InArchive,
                    TreePath = kvp.Value.TreePath,
                    ParentPath = kvp.Value.ParentPath,
                    Length = kvp.Value.Length
                };

                foreach (KeyValuePair<int, Tuples> kvpI in kvp.Value.Tuples)
                {
                    Tuples index = new Tuples
                    {
                        Length = kvpI.Value.Length,
                        Offset = kvpI.Value.Offset,
                        Prefix = kvpI.Value.Prefix
                    };

                    archIndex.Tuples.Add(kvpI.Key, index);
                }

                indexCopy.Add(kvp.Key, archIndex);
            }

            return indexCopy;
        }

        public Stream ExtractData(string fileName)
        {
            if (!Index.ContainsKey(fileName))
            {
                throw new FileNotFoundException("Specified file does not exist in RenPy Archive.");
            }

            if (Index[fileName].InArchive)
            {
                return new RpaStream(Index[fileName], this);
            }

            return File.OpenRead(Index[fileName].FullPath);
        }

        public string Extract(string fileName, string exportPath)
        {
            string finalPath;
            if (exportPath.Trim() == String.Empty)
            {
                finalPath = ArchiveInfo.DirectoryName + @"\" + fileName;
            }
            else
            {
                if (!Directory.Exists(exportPath.Trim()))
                {
                    throw new Exception("Selected export path does not exist.");
                }
                finalPath = exportPath.Trim() + @"\" + fileName;
            }

            Directory.CreateDirectory(Path.GetDirectoryName(finalPath) ?? throw new InvalidOperationException());
            using (var source = ExtractData(fileName))
            {
                using (var dest = File.OpenWrite(finalPath))
                {
                    source.CopyTo(dest);
                }
            }

            return ArchiveInfo.DirectoryName + @"\" + fileName;
        }

        public string SaveArchive(string archivePath)
        {
            if (archivePath.ToLower().EndsWith(".rpi"))
            {
                archivePath = Regex.Replace(archivePath, @"\.rpi$", ".rpa", RegexOptions.IgnoreCase);
            }

            if (!archivePath.ToLower().EndsWith(".rpa"))
            {
                archivePath += ".rpa";
            }

            string tmpPath = Regex.Replace(archivePath, @"\.rpa$", "", RegexOptions.IgnoreCase);
            tmpPath = tmpPath.Substring(0, Math.Min(100, tmpPath.Length - 1)) + "_" +
                      DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString("N");

            /*if (archivePath == _archivePath && _archivePath != String.Empty)
            {
                throw new Exception("Cannot overwrite same archive file that is loaded.");
            }*/

            string indexPath = Regex.Replace(archivePath, @"\.rpa$", ".rpi", RegexOptions.IgnoreCase);

            /*if (indexPath == _indexPath && _indexPath != String.Empty)
            {
                throw new Exception("Cannot overwrite same index file that is loaded.");
            }*/

            BuildArchive(archivePath, indexPath, tmpPath);

            return archivePath;
        }

        public event ProgressChangedEventHandler SaveProgress;

        private void BuildArchive(string archivePath, string indexPath, string tmpPath)
        {
            try
            {
                using (Stream stream = File.Open(tmpPath + ".rpa", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    long archiveOffset;
                    switch (ArchiveVersion)
                    {
                        case Version.RPA_3_2:
                            archiveOffset = 34;
                            break;
                        case Version.RPA_3:
                            archiveOffset = 34;
                            break;
                        case Version.RPA_2:
                            archiveOffset = 25;
                            break;
                        case Version.RPA_1:
                            archiveOffset = 0;
                            break;
                        default:
                            throw new Exception("Specified version is not supported.");
                    }

                    stream.Position = archiveOffset;

                    Random rnd = new Random();

                    // Update indexes
                    //TODO: >2GB support
                    Hashtable indexes = new Hashtable();
                    double i = 0;
                    foreach (KeyValuePair<string, ArchiveIndex> index in Index)
                    {
                        i++;
                        var ms = new MemoryStream();
                        using (var source = ExtractData(index.Key))
                        {
                            source.CopyTo(ms);
                        }

                        byte[] content = ms.ToArray();

                        if (Padding > 0)
                        {
                            string paddingStr = String.Empty;
                            int paddingLength = rnd.Next(1, Padding);

                            while (paddingLength > 0)
                            {
                                paddingStr += Encoding.ASCII.GetString(new[] { (byte)rnd.Next(1, 255) });
                                paddingLength--;
                            }

                            byte[] paddingBytes = Encoding.ASCII.GetBytes(paddingStr);
                            archiveOffset += paddingBytes.Length;
                        }

                        stream.Position = archiveOffset;
                        stream.Write(content, 0, content.Length);
                        SaveProgress?.Invoke(this, new ProgressChangedEventArgs((int)(i / Index.Count * 100), index.Value));

                        List<object[]> indexData = new List<object[]>();
                        if (CheckVersion(ArchiveVersion, Version.RPA_3) ||
                            CheckVersion(ArchiveVersion, Version.RPA_3_2))
                        {
                            indexData.Add(new object[]
                                {archiveOffset ^ ObfuscationKey, content.Length ^ ObfuscationKey, ""}); // Last is prefix
                        }
                        else
                        {
                            indexData.Add(new object[] { archiveOffset, content.Length });
                        }

                        archiveOffset += content.Length;

                        indexes.Add(index.Value.TreePath, indexData);
                    }

                    byte[] pickledIndexes;
                    using (Pickler pickler = new Pickler())
                    {
                        pickledIndexes = pickler.dumps(indexes);
                    }

                    byte[] fileCompressed = ZlibStream.CompressBuffer(pickledIndexes);

                    if (!CheckVersion(ArchiveVersion, Version.RPA_1))
                    {
                        stream.Position = archiveOffset;
                        stream.Write(fileCompressed, 0, fileCompressed.Length);

                        string headerContent = String.Empty;

                        switch (ArchiveVersion)
                        {
                            case Version.RPA_3_2:
                                headerContent = ArchiveMagic.RPA_3_2 + archiveOffset.ToString("x").PadLeft(16, '0') +
                                                " " +
                                                ObfuscationKey.ToString("x").PadLeft(8, '0') + "\n";
                                break;
                            case Version.RPA_3:
                                headerContent = ArchiveMagic.RPA_3 + archiveOffset.ToString("x").PadLeft(16, '0') +
                                                " " +
                                                ObfuscationKey.ToString("x").PadLeft(8, '0') + "\n";
                                break;
                            case Version.RPA_2:
                                headerContent = ArchiveMagic.RPA_2 + archiveOffset.ToString("x").PadLeft(16, '0') +
                                                "\n";
                                break;
                        }

                        byte[] headerContentByte = Encoding.UTF8.GetBytes(headerContent);

                        stream.Position = 0;
                        stream.Write(headerContentByte, 0, headerContentByte.Length);
                    }
                    else
                    {
                        File.WriteAllBytes(tmpPath + ".rpi", fileCompressed);
                    }
                }

                try
                {
                    // Test if archive is corrupted or not
                    RpaParser testParse = new RpaParser();
                    testParse.LoadArchive(tmpPath + ".rpa");
                }
                catch (Exception ex)
                {
                    throw new Exception("Validation of newly created archive failed. This usually means corrupted archive file after creation. No harm was done to original archive. Parser failed with following error during validation: " + ex.Message);
                }

                File.Copy(tmpPath + ".rpa", archivePath, true);
                File.Delete(tmpPath + ".rpa");
                if (File.Exists(tmpPath + ".rpi"))
                {
                    File.Copy(tmpPath + ".rpi", indexPath, true);
                    File.Delete(tmpPath + ".rpi");
                }
            }
            catch
            {
                if (File.Exists(tmpPath + ".rpa"))
                {
                    File.Delete(tmpPath + ".rpa");
                }
                if (File.Exists(tmpPath + ".rpi"))
                {
                    File.Delete(tmpPath + ".rpi");
                }

                throw;
            }
        }

        private class RpaStream : Stream
        {
            private readonly ArchiveIndex entry;
            private readonly BinaryReader reader;

            private int tupleIndex = 0;
            private int tuplePosition = 0;
            private bool inPrefix = true;
            private long absolutePosition = 0;

            public RpaStream(ArchiveIndex entry, RpaParser parser)
            {
                this.entry = entry;
                reader = new BinaryReader(File.OpenRead(parser._archivePath), Encoding.UTF8);
            }

            public override bool CanRead => true;

            public override bool CanSeek => true;

            public override bool CanWrite => false;

            public override long Length => entry.Length;

            public override long Position { get => absolutePosition; set => Seek(value, SeekOrigin.Begin); }

            public override void Flush() { return; }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (tupleIndex >= entry.Tuples.Count) return 0;
                int bytesRead = count;
                if (inPrefix)
                {
                    var remainingInPrefix = entry.Tuples[tupleIndex].Prefix.Length - tuplePosition;
                    if (remainingInPrefix < bytesRead) bytesRead = remainingInPrefix;
                    Array.Copy(entry.Tuples[tupleIndex].Prefix, tuplePosition, buffer, offset, bytesRead);
                    tuplePosition += bytesRead;
                    absolutePosition += bytesRead;
                    if (tuplePosition == entry.Tuples[tupleIndex].Prefix.Length)
                    {
                        tuplePosition = 0;
                        inPrefix = false;
                    }
                }
                else
                {
                    var remainingInTuple = entry.Tuples[tupleIndex].Length - entry.Tuples[tupleIndex].Prefix.Length - tuplePosition;
                    if (remainingInTuple < bytesRead) bytesRead = (int)remainingInTuple;
                    reader.BaseStream.Seek(entry.Tuples[tupleIndex].Offset + tuplePosition, SeekOrigin.Begin);
                    bytesRead = reader.Read(buffer, offset, bytesRead);
                    tuplePosition += bytesRead;
                    absolutePosition += bytesRead;
                    if (tuplePosition == entry.Tuples[tupleIndex].Length - entry.Tuples[tupleIndex].Prefix.Length)
                    {
                        tupleIndex++;
                        tuplePosition = 0;
                        inPrefix = true;
                    }
                }
                if (bytesRead < count && absolutePosition < entry.Length)
                    return bytesRead + Read(buffer, offset + bytesRead, count - bytesRead);
                return bytesRead;
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                long desiredPosition = offset;
                if (origin == SeekOrigin.Current) desiredPosition += absolutePosition;
                if (origin == SeekOrigin.End) desiredPosition += entry.Length;
                if (desiredPosition < 0) desiredPosition = 0;
                if (desiredPosition > entry.Length) desiredPosition = entry.Length;
                absolutePosition = desiredPosition;
                tupleIndex = 0;
                tuplePosition = 0;
                inPrefix = true;
                if(desiredPosition == entry.Length)
                {
                    tupleIndex = entry.Tuples.Count;
                    return entry.Length;
                }
                while (entry.Tuples[tupleIndex].Length <= desiredPosition)
                {
                    desiredPosition -= entry.Tuples[tupleIndex].Length; tupleIndex++;
                }
                if (tupleIndex == entry.Tuples.Count) return absolutePosition;
                if (desiredPosition < entry.Tuples[tupleIndex].Prefix.Length)
                {
                    tuplePosition = (int)desiredPosition;
                }
                else
                {
                    inPrefix = false;
                    tuplePosition = (int)desiredPosition - entry.Tuples[tupleIndex].Prefix.Length;
                }

                return absolutePosition;
            }

            public override void SetLength(long value)
            {
                throw new NotImplementedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotImplementedException();
            }

            protected override void Dispose(bool disposing)
            {
                reader.Dispose();
                base.Dispose(disposing);
            }
        }

    }

    // https://stackoverflow.com/a/28418846/3650856
    public class StructConverter
    {
        // We use this function to provide an easier way to make type-agnostic call via GetBytes method of the BitConverter class.
        // This means we can have much cleaner code below.
        private static byte[] TypeAgnosticGetBytes(object o)
        {
            switch (o)
            {
                case char c:
                    return BitConverter.GetBytes(c);
                case int i:
                    return BitConverter.GetBytes(i);
                case uint u:
                    return BitConverter.GetBytes(u);
                case long l:
                    return BitConverter.GetBytes(l);
                case ulong @ulong:
                    return BitConverter.GetBytes(@ulong);
                case short s:
                    return BitConverter.GetBytes(s);
                case ushort @ushort:
                    return BitConverter.GetBytes(@ushort);
                case byte _:
                case sbyte _:
                    return new[] { (byte)o };
                default:
                    throw new ArgumentException("Unsupported object type found");
            }
        }

        private static string GetFormatSpecifierFor(object o)
        {
            switch (o)
            {
                case char _:
                    return "c";
                case int _:
                    return "i";
                case uint _:
                    return "I";
                case long _:
                    return "q";
                case ulong _:
                    return "Q";
                case short _:
                    return "h";
                case ushort _:
                    return "H";
                case byte _:
                    return "B";
                case sbyte _:
                    return "b";
                default:
                    throw new ArgumentException("Unsupported object type found");
            }
        }

        /// <summary>
        /// Convert a byte array into an array of objects based on Python's "struct.unpack" protocol.
        /// </summary>
        /// <param name="fmt">A "struct.pack"-compatible format string</param>
        /// <param name="bytes">An array of bytes to convert to objects</param>
        /// <returns>Array of objects.</returns>
        /// <remarks>You are responsible for casting the objects in the array back to their proper types.</remarks>
        public static object[] Unpack(string fmt, byte[] bytes)
        {
            Debug.WriteLine("Format string is length {0}, {1} bytes provided.", fmt.Length, bytes.Length);

            // First we parse the format string to make sure it's proper.
            if (fmt.Length < 1) throw new ArgumentException("Format string cannot be empty.");

            bool endianFlip = false;
            if (fmt.Substring(0, 1) == "<")
            {
                Debug.WriteLine("  Endian marker found: little endian");
                // Little endian.
                // Do we need to flip endianness?
                if (BitConverter.IsLittleEndian == false) endianFlip = true;
                fmt = fmt.Substring(1);
            }
            else if (fmt.Substring(0, 1) == ">")
            {
                Debug.WriteLine("  Endian marker found: big endian");
                // Big endian.
                // Do we need to flip endianness?
                if (BitConverter.IsLittleEndian) endianFlip = true;
                fmt = fmt.Substring(1);
            }

            // Now, we find out how long the byte array needs to be
            int totalByteLength = 0;
            foreach (char c in fmt)
            {
                Debug.WriteLine("  Format character found: {0}", c);
                switch (c)
                {
                    case 'q':
                    case 'Q':
                        totalByteLength += 8;
                        break;
                    case 'i':
                    case 'I':
                        totalByteLength += 4;
                        break;
                    case 'h':
                    case 'H':
                        totalByteLength += 2;
                        break;
                    case 'b':
                    case 'B':
                    case 'x':
                        totalByteLength += 1;
                        break;
                    default:
                        throw new ArgumentException("Invalid character found in format string.");
                }
            }

            Debug.WriteLine("Endianness will {0}be flipped.", (object)(endianFlip ? "" : "NOT "));
            Debug.WriteLine("The byte array is expected to be {0} bytes long.", totalByteLength);

            // Test the byte array length to see if it contains as many bytes as is needed for the string.
            if (bytes.Length != totalByteLength) throw new ArgumentException("The number of bytes provided does not match the total length of the format string.");

            // Ok, we can go ahead and start parsing bytes!
            int byteArrayPosition = 0;
            var outputList = new List<object>();

            Debug.WriteLine("Processing byte array...");
            foreach (char c in fmt)
            {
                byte[] buf;
                switch (c)
                {
                    case 'q':
                        outputList.Add(BitConverter.ToInt64(bytes, byteArrayPosition));
                        byteArrayPosition += 8;
                        Debug.WriteLine("  Added signed 64-bit integer.");
                        break;
                    case 'Q':
                        outputList.Add(BitConverter.ToUInt64(bytes, byteArrayPosition));
                        byteArrayPosition += 8;
                        Debug.WriteLine("  Added unsigned 64-bit integer.");
                        break;
                    case 'i':
                        outputList.Add(BitConverter.ToInt32(bytes, byteArrayPosition));
                        byteArrayPosition += 4;
                        Debug.WriteLine("  Added signed 32-bit integer.");
                        break;
                    case 'I':
                        outputList.Add(BitConverter.ToUInt32(bytes, byteArrayPosition));
                        byteArrayPosition += 4;
                        Debug.WriteLine("  Added unsigned 32-bit integer.");
                        break;
                    case 'h':
                        outputList.Add(BitConverter.ToInt16(bytes, byteArrayPosition));
                        byteArrayPosition += 2;
                        Debug.WriteLine("  Added signed 16-bit integer.");
                        break;
                    case 'H':
                        if (endianFlip)
                        {
                            var deezBytes = bytes.Skip(byteArrayPosition).Take(2).ToArray();
                            deezBytes.Reverse();
                            outputList.Add(BitConverter.ToUInt16(deezBytes, 0));
                        }
                        else
                        {
                            outputList.Add(BitConverter.ToUInt16(bytes, byteArrayPosition));
                        }

                        byteArrayPosition += 2;
                        Debug.WriteLine("  Added unsigned 16-bit integer.");
                        break;
                    case 'b':
                        buf = new byte[1];
                        Array.Copy(bytes, byteArrayPosition, buf, 0, 1);
                        outputList.Add((sbyte)buf[0]);
                        byteArrayPosition++;
                        Debug.WriteLine("  Added signed byte");
                        break;
                    case 'B':
                        buf = new byte[1];
                        Array.Copy(bytes, byteArrayPosition, buf, 0, 1);
                        outputList.Add(buf[0]);
                        byteArrayPosition++;
                        Debug.WriteLine("  Added unsigned byte");
                        break;
                    case 'x':
                        byteArrayPosition++;
                        Debug.WriteLine("  Ignoring a byte");
                        break;
                    default:
                        throw new ArgumentException("You should not be here.");
                }
            }
            return outputList.ToArray();
        }

        /// <summary>
        /// Convert an array of objects to a byte array, along with a string that can be used with Unpack.
        /// </summary>
        /// <param name="items">An object array of items to convert</param>
        /// <param name="littleEndian">Set to False if you want to use big endian output.</param>
        /// <param name="neededFormatStringToRecover">Variable to place an 'Unpack'-compatible format string into.</param>
        /// <returns>A Byte array containing the objects provided in binary format.</returns>
        public static byte[] Pack(object[] items, bool littleEndian, out string neededFormatStringToRecover)
        {

            // make a byte list to hold the bytes of output
            var outputBytes = new List<byte>();

            // should we be flipping bits for proper endianness?
            bool endianFlip = (littleEndian != BitConverter.IsLittleEndian);

            // start working on the output string
            string outString = (littleEndian == false ? ">" : "<");

            // convert each item in the objects to the representative bytes
            foreach (object o in items)
            {
                byte[] theseBytes = TypeAgnosticGetBytes(o);
                if (endianFlip) theseBytes.Reverse();
                outString += GetFormatSpecifierFor(o);
                outputBytes.AddRange(theseBytes);
            }

            neededFormatStringToRecover = outString;

            return outputBytes.ToArray();

        }

        public static byte[] Pack(object[] items)
        {
            string dummy = "";
            return Pack(items, true, out dummy);
        }
    }

}
