// this file has been automatically generated, DO NOT EDIT

namespace YAF.Lucene.Net.Util.Packed
{
    /*
     * Licensed to the Apache Software Foundation (ASF) under one or more
     * contributor license agreements.  See the NOTICE file distributed with
     * this work for additional information regarding copyright ownership.
     * The ASF licenses this file to You under the Apache License, Version 2.0
     * (the "License"); you may not use this file except in compliance with
     * the License.  You may obtain a copy of the License at
     *
     *     http://www.apache.org/licenses/LICENSE-2.0
     *
     * Unless required by applicable law or agreed to in writing, software
     * distributed under the License is distributed on an "AS IS" BASIS,
     * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     * See the License for the specific language governing permissions and
     * limitations under the License.
     */

    /// <summary>
    /// Efficient sequential read/write of packed integers.
    /// </summary>
    internal sealed class BulkOperationPacked5 : BulkOperationPacked
    {
        public BulkOperationPacked5()
            : base(5)
        {
        }

        public override void Decode(long[] blocks, int blocksOffset, int[] values, int valuesOffset, int iterations)
        {
            for (int i = 0; i < iterations; ++i)
            {
                long block0 = blocks[blocksOffset++];
                values[valuesOffset++] = (int)((long)((ulong)block0 >> 59));
                values[valuesOffset++] = (int)(((long)((ulong)block0 >> 54)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block0 >> 49)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block0 >> 44)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block0 >> 39)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block0 >> 34)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block0 >> 29)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block0 >> 24)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block0 >> 19)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block0 >> 14)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block0 >> 9)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block0 >> 4)) & 31L);
                long block1 = blocks[blocksOffset++];
                values[valuesOffset++] = (int)(((block0 & 15L) << 1) | ((long)((ulong)block1 >> 63)));
                values[valuesOffset++] = (int)(((long)((ulong)block1 >> 58)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block1 >> 53)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block1 >> 48)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block1 >> 43)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block1 >> 38)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block1 >> 33)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block1 >> 28)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block1 >> 23)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block1 >> 18)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block1 >> 13)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block1 >> 8)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block1 >> 3)) & 31L);
                long block2 = blocks[blocksOffset++];
                values[valuesOffset++] = (int)(((block1 & 7L) << 2) | ((long)((ulong)block2 >> 62)));
                values[valuesOffset++] = (int)(((long)((ulong)block2 >> 57)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block2 >> 52)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block2 >> 47)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block2 >> 42)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block2 >> 37)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block2 >> 32)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block2 >> 27)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block2 >> 22)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block2 >> 17)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block2 >> 12)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block2 >> 7)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block2 >> 2)) & 31L);
                long block3 = blocks[blocksOffset++];
                values[valuesOffset++] = (int)(((block2 & 3L) << 3) | ((long)((ulong)block3 >> 61)));
                values[valuesOffset++] = (int)(((long)((ulong)block3 >> 56)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block3 >> 51)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block3 >> 46)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block3 >> 41)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block3 >> 36)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block3 >> 31)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block3 >> 26)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block3 >> 21)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block3 >> 16)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block3 >> 11)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block3 >> 6)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block3 >> 1)) & 31L);
                long block4 = blocks[blocksOffset++];
                values[valuesOffset++] = (int)(((block3 & 1L) << 4) | ((long)((ulong)block4 >> 60)));
                values[valuesOffset++] = (int)(((long)((ulong)block4 >> 55)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block4 >> 50)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block4 >> 45)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block4 >> 40)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block4 >> 35)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block4 >> 30)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block4 >> 25)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block4 >> 20)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block4 >> 15)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block4 >> 10)) & 31L);
                values[valuesOffset++] = (int)(((long)((ulong)block4 >> 5)) & 31L);
                values[valuesOffset++] = (int)(block4 & 31L);
            }
        }

        public override void Decode(byte[] blocks, int blocksOffset, int[] values, int valuesOffset, int iterations)
        {
            for (int i = 0; i < iterations; ++i)
            {
                int byte0 = blocks[blocksOffset++] & 0xFF;
                values[valuesOffset++] = (int)((uint)byte0 >> 3);
                int byte1 = blocks[blocksOffset++] & 0xFF;
                values[valuesOffset++] = ((byte0 & 7) << 2) | ((int)((uint)byte1 >> 6));
                values[valuesOffset++] = ((int)((uint)byte1 >> 1)) & 31;
                int byte2 = blocks[blocksOffset++] & 0xFF;
                values[valuesOffset++] = ((byte1 & 1) << 4) | ((int)((uint)byte2 >> 4));
                int byte3 = blocks[blocksOffset++] & 0xFF;
                values[valuesOffset++] = ((byte2 & 15) << 1) | ((int)((uint)byte3 >> 7));
                values[valuesOffset++] = ((int)((uint)byte3 >> 2)) & 31;
                int byte4 = blocks[blocksOffset++] & 0xFF;
                values[valuesOffset++] = ((byte3 & 3) << 3) | ((int)((uint)byte4 >> 5));
                values[valuesOffset++] = byte4 & 31;
            }
        }

        public override void Decode(long[] blocks, int blocksOffset, long[] values, int valuesOffset, int iterations)
        {
            for (int i = 0; i < iterations; ++i)
            {
                long block0 = blocks[blocksOffset++];
                values[valuesOffset++] = (long)((ulong)block0 >> 59);
                values[valuesOffset++] = ((long)((ulong)block0 >> 54)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block0 >> 49)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block0 >> 44)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block0 >> 39)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block0 >> 34)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block0 >> 29)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block0 >> 24)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block0 >> 19)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block0 >> 14)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block0 >> 9)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block0 >> 4)) & 31L;
                long block1 = blocks[blocksOffset++];
                values[valuesOffset++] = ((block0 & 15L) << 1) | ((long)((ulong)block1 >> 63));
                values[valuesOffset++] = ((long)((ulong)block1 >> 58)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block1 >> 53)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block1 >> 48)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block1 >> 43)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block1 >> 38)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block1 >> 33)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block1 >> 28)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block1 >> 23)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block1 >> 18)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block1 >> 13)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block1 >> 8)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block1 >> 3)) & 31L;
                long block2 = blocks[blocksOffset++];
                values[valuesOffset++] = ((block1 & 7L) << 2) | ((long)((ulong)block2 >> 62));
                values[valuesOffset++] = ((long)((ulong)block2 >> 57)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block2 >> 52)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block2 >> 47)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block2 >> 42)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block2 >> 37)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block2 >> 32)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block2 >> 27)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block2 >> 22)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block2 >> 17)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block2 >> 12)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block2 >> 7)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block2 >> 2)) & 31L;
                long block3 = blocks[blocksOffset++];
                values[valuesOffset++] = ((block2 & 3L) << 3) | ((long)((ulong)block3 >> 61));
                values[valuesOffset++] = ((long)((ulong)block3 >> 56)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block3 >> 51)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block3 >> 46)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block3 >> 41)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block3 >> 36)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block3 >> 31)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block3 >> 26)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block3 >> 21)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block3 >> 16)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block3 >> 11)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block3 >> 6)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block3 >> 1)) & 31L;
                long block4 = blocks[blocksOffset++];
                values[valuesOffset++] = ((block3 & 1L) << 4) | ((long)((ulong)block4 >> 60));
                values[valuesOffset++] = ((long)((ulong)block4 >> 55)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block4 >> 50)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block4 >> 45)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block4 >> 40)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block4 >> 35)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block4 >> 30)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block4 >> 25)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block4 >> 20)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block4 >> 15)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block4 >> 10)) & 31L;
                values[valuesOffset++] = ((long)((ulong)block4 >> 5)) & 31L;
                values[valuesOffset++] = block4 & 31L;
            }
        }

        public override void Decode(byte[] blocks, int blocksOffset, long[] values, int valuesOffset, int iterations)
        {
            for (int i = 0; i < iterations; ++i)
            {
                long byte0 = blocks[blocksOffset++] & 0xFF;
                values[valuesOffset++] = (long)((ulong)byte0 >> 3);
                long byte1 = blocks[blocksOffset++] & 0xFF;
                values[valuesOffset++] = ((byte0 & 7) << 2) | ((long)((ulong)byte1 >> 6));
                values[valuesOffset++] = ((long)((ulong)byte1 >> 1)) & 31;
                long byte2 = blocks[blocksOffset++] & 0xFF;
                values[valuesOffset++] = ((byte1 & 1) << 4) | ((long)((ulong)byte2 >> 4));
                long byte3 = blocks[blocksOffset++] & 0xFF;
                values[valuesOffset++] = ((byte2 & 15) << 1) | ((long)((ulong)byte3 >> 7));
                values[valuesOffset++] = ((long)((ulong)byte3 >> 2)) & 31;
                long byte4 = blocks[blocksOffset++] & 0xFF;
                values[valuesOffset++] = ((byte3 & 3) << 3) | ((long)((ulong)byte4 >> 5));
                values[valuesOffset++] = byte4 & 31;
            }
        }
    }
}