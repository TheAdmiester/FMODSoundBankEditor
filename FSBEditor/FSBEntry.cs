using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSBEditor
{
    class FSBEntry
    {
        public string name, xmaName;
        public short size;
        public int numSamples, streamSize, loopStartSample, loopEndSample, sampleRate, volume, unknownInt;
        public long startOffset;
        public short pan, defPri, numChannels;
        public byte[] audioData, unknownData;
        public byte codec;

        public FSBEntry()
        {
            codec = 1;
            loopStartSample = 0;
            defPri = 128;
            pan = 255;
            volume = 112;
            numChannels = 1;
            unknownInt = 0;
        }
    }
}
