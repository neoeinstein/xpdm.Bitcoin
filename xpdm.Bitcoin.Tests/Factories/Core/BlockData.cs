using System.Collections.Generic;
using System.Reflection;
using Gallio.Framework;
using Gallio.Framework.Data;
using xpdm.Bitcoin.Core;
using xpdm.Bitcoin.Tests.Factories.Core.Blocks;

namespace xpdm.Bitcoin.Tests.Factories.Core
{
    class BlockData
    {
        #region Block Factories

        public static IEnumerable<IDataItem> BlocksForSerialization
        {
            get
            {
                foreach (var testBlock in BlockData.Instance._blocksHash.Values)
                {
                    if (testBlock.SerializedBlockData != null)
                    {
                        yield return new DataRow(
                            testBlock.SerializedBlockData,
                            0,
                            testBlock.Header,
                            testBlock.ExpectedHash,
                            testBlock.ExpectedMerkleTree
                            );
                    }
                }
            }
        }

        public static IEnumerable<IDataItem> BlockTuples
        {
            get
            {
                foreach (var testBlock in BlockData.Instance._blocksHash.Values)
                {
                    if (testBlock.Block != null)
                    {
                        yield return new DataRow(
                            testBlock.Block,
                            testBlock.Header,
                            testBlock.ExpectedHash,
                            testBlock.ExpectedMerkleTree
                            );
                    }
                }
            }
        }

        #endregion

        public static byte[] GetSerializedData(string dataResourceName)
        {
            dataResourceName = Assembly.GetExecutingAssembly().GetName().Name + ".SerializedData." + dataResourceName;
            using (var resStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(dataResourceName))
            {
                if (resStream == null)
                {
                    DiagnosticLog.WriteLine("Unable to find for: {0} among:", dataResourceName);
                    DiagnosticLog.WriteLine(string.Join("\n", System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames()));
                    return null;
                }
                using (var reader = new System.IO.BinaryReader(resStream))
                {
                    return reader.ReadBytes((int)reader.BaseStream.Length);
                }
            }
        }

        private C5.IDictionary<BitcoinObject, ITestBlock> _blocksHash = new C5.HashDictionary<BitcoinObject, ITestBlock>
        {
            { B000000.Instance.ExpectedHash, B000000.Instance },
            { B000170.Instance.ExpectedHash, B000170.Instance },
            { B072783.Instance.ExpectedHash, B072783.Instance },
            { B072785.Instance.ExpectedHash, B072785.Instance },
            { B103640.Instance.ExpectedHash, B103640.Instance },
            { B103958.Instance.ExpectedHash, B103958.Instance },
            { B124009.Instance.ExpectedHash, B124009.Instance },
            { B124010.Instance.ExpectedHash, B124010.Instance },
        };

        private C5.IDictionary<int, ITestBlock> _blockNbrHash = new C5.HashDictionary<int, ITestBlock>
        {
            { 0, B000000.Instance },
            { 170, B000170.Instance },
            { 72783, B072783.Instance },
            { 72785, B072785.Instance },
            { 103640, B103640.Instance },
            { 103958, B103958.Instance },
            { 124009, B124009.Instance },
            { 124010, B124010.Instance },
        };

        public ITestBlock this[string blockHash]
        {
            get { return _blocksHash[Hash256.Parse(blockHash)]; }
        }

        public ITestBlock this[BitcoinObject bitObj]
        {
            get { return _blocksHash[bitObj]; }
        }

        public ITestBlock this[int blockNbr]
        {
            get { return _blockNbrHash[blockNbr]; }
        }

        private static BlockData _instance = new BlockData();
        public static BlockData Instance
        {
            get { return _instance; }
        }

        private BlockData()
        {
        }
    }
}
