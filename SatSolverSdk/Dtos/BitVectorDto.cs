using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace SatSolverSdk.Dtos
{
    public class BitVectorDto
    {
        public BitVectorDto(int size, bool defaultValue)
        {
            var collection = (int) Math.Ceiling((double)size / 32);
            Data = Enumerable.Repeat(new BitVector32(defaultValue ? Int32.MaxValue : 0), collection).ToList();
        }

        public BitVectorDto(BitVectorDto vector)
        {
            Data = new List<BitVector32>();
            foreach (var bitVector32 in vector.Data)
            {
                Data.Add(new BitVector32(bitVector32));
            }
        }

        private IList<BitVector32> Data { get; }

        public bool this[int index]
        {
            get
            {
                var collection = index / 32;
                index = index % 32;
                index = 1 << index;
                
                return Data[collection][index];
            }
            set
            {
                var collection = index / 32;
                index = index % 32;
                index = 1 << index;

                var bitVector32 = Data[collection];
                bitVector32[index] = value;
                Data[collection] = new BitVector32(bitVector32);
            }
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (var vector32 in Data)
            {
                hash = hash * 23 + vector32.GetHashCode();
            }
            return hash;
        }

    }

}