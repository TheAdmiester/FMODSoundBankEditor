using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FSBEditor
{
    class HarmonicTuning
    {
        public static void WriteXML(string path, string fileName, FSBFile fsb)
        {
            using (var xml = XmlWriter.Create(string.Format("{0}_HT.xml", Path.Combine(path, fileName.ToUpper())), new XmlWriterSettings { Indent = true }))
            {
                string soundType = "";
                xml.WriteStartDocument();
                xml.WriteStartElement("Soundbank");
                xml.WriteAttributeString("name", string.Format("{0}.fsb", fileName));
                
                if (fileName.ToLower().Contains("_exh"))
                {
                    soundType = "ExhaustL";
                }
                else if (fileName.ToLower().Contains("_engamb"))
                {
                    soundType = "EngineAmbient";
                }
                else if (fileName.ToLower().Contains("_int"))
                {
                    soundType = "EngineIntake";
                }

                xml.WriteStartElement(soundType);
                    WriteBankLoops(xml, fsb);
                xml.WriteEndElement();

                if (soundType == "ExhaustL")
                {
                    // Write another loop bank for stereo
                    xml.WriteStartElement("ExhaustR");
                        WriteBankLoops(xml, fsb);
                    xml.WriteEndElement();

                    xml.WriteStartElement("EngineSettings");
                        xml.WriteAttributeString("audio_rpm_idle_gain_reset", "850");
                    xml.WriteEndElement();

                    xml.WriteStartElement("ExhaustNoise");
                        xml.WriteAttributeString("Volume", "1.000000");
                    xml.WriteEndElement();

                    xml.WriteStartElement("Burble");
                        xml.WriteStartElement("Wavebank");
                            xml.WriteAttributeString("name", "SequencePops_2.fsb");
                            xml.WriteAttributeString("burbles", "2");
                            xml.WriteAttributeString("backfires", "3");
                        xml.WriteEndElement();
                        xml.WriteStartElement("Volume");
                            xml.WriteStartElement("PhysicsCoeff");
                                xml.WriteAttributeString("RPM", "0.300000");
                                xml.WriteAttributeString("Throttle", "0.000000");
                                xml.WriteAttributeString("PosTorque", "0.000000");
                                xml.WriteAttributeString("NegTorque", "0.700000");
                            xml.WriteEndElement();
                            xml.WriteStartElement("ThreePointCurve");
                                xml.WriteAttributeString("x0", "0.100000");
                                xml.WriteAttributeString("y0", "0.100000");
                                xml.WriteAttributeString("x1", "0.700000");
                                xml.WriteAttributeString("y1", "0.700000");
                                xml.WriteAttributeString("x2", "1.000000");
                                xml.WriteAttributeString("y2", "1.000000");
                            xml.WriteEndElement();
                        xml.WriteEndElement();
                        xml.WriteStartElement("Timer");
                        xml.WriteAttributeString("MinTime", "29.000000");
                        xml.WriteAttributeString("MaxTime", "802.000000");
                        xml.WriteAttributeString("MinRPM", "2000.000000");
                        xml.WriteAttributeString("MaxRPM", "20000.000000");
                        xml.WriteAttributeString("SilenceWeighting", "5");
                        xml.WriteEndElement();
                    xml.WriteEndElement();
                }

                xml.WriteStartElement("DSP");
                    xml.WriteStartElement("Volume");
                        xml.WriteStartElement("Gain");
                            xml.WriteStartElement("PhysicsCoeff");
                                xml.WriteAttributeString("RPM", "0.250000");
                                xml.WriteAttributeString("Throttle", "0.750000");
                                xml.WriteAttributeString("PosTorque", "0.000000");
                                xml.WriteAttributeString("NegTorque", "0.000000");
                            xml.WriteEndElement();
                            xml.WriteStartElement("ThreePointCurve");
                                xml.WriteAttributeString("x0", "0.000000");
                                xml.WriteAttributeString("y0", "0.500000");
                                xml.WriteAttributeString("x1", "0.500000");
                                xml.WriteAttributeString("y1", "0.550000");
                                xml.WriteAttributeString("x2", "1.000000");
                                xml.WriteAttributeString("y2", "0.600000");
                            xml.WriteEndElement();
                        xml.WriteEndElement();
                    xml.WriteEndElement();
                    xml.WriteStartElement("Expander");
                        xml.WriteStartElement("MaxGain");
                            xml.WriteStartElement("PhysicsCoeff");
                                xml.WriteAttributeString("RPM", "1.000000");
                                xml.WriteAttributeString("Throttle", "0.000000");
                                xml.WriteAttributeString("PosTorque", "0.000000");
                                xml.WriteAttributeString("NegTorque", "0.000000");
                            xml.WriteEndElement();
                            xml.WriteStartElement("ThreePointCurve");
                                xml.WriteAttributeString("x0", "0.000000");
                                xml.WriteAttributeString("y0", "0.170000");
                                xml.WriteAttributeString("x1", "0.600000");
                                xml.WriteAttributeString("y1", "0.700000");
                                xml.WriteAttributeString("x2", "0.950000");
                                xml.WriteAttributeString("y2", "1.000000");
                            xml.WriteEndElement();
                        xml.WriteEndElement();
                    xml.WriteEndElement();
                    xml.WriteStartElement("PEQ");
                        xml.WriteAttributeString("Active", "1");
                        xml.WriteStartElement("Gain");
                            xml.WriteStartElement("PhysicsCoeff");
                                xml.WriteAttributeString("RPM", "0.000000");
                                xml.WriteAttributeString("Throttle", "1.000000");
                                xml.WriteAttributeString("PosTorque", "0.000000");
                                xml.WriteAttributeString("NegTorque", "0.000000");
                            xml.WriteEndElement();
                            xml.WriteStartElement("ThreePointCurve");
                                xml.WriteAttributeString("x0", "0.000000");
                                xml.WriteAttributeString("y0", "0.600000");
                                xml.WriteAttributeString("x1", "0.500000");
                                xml.WriteAttributeString("y1", "0.820000");
                                xml.WriteAttributeString("x2", "1.000000");
                                xml.WriteAttributeString("y2", "1.000000");
                            xml.WriteEndElement();
                        xml.WriteEndElement();
                        xml.WriteStartElement("CenterFrequency");
                            xml.WriteStartElement("PhysicsCoeff");
                                xml.WriteAttributeString("RPM", "0.000000");
                                xml.WriteAttributeString("Throttle", "0.000000");
                                xml.WriteAttributeString("PosTorque", "0.000000");
                                xml.WriteAttributeString("NegTorque", "0.000000");
                            xml.WriteEndElement();
                            xml.WriteStartElement("ThreePointCurve");
                                xml.WriteAttributeString("x0", "0.000000");
                                xml.WriteAttributeString("y0", "8807.000000");
                                xml.WriteAttributeString("x1", "1.000000");
                                xml.WriteAttributeString("y1", "20.000000");
                                xml.WriteAttributeString("x2", "1.000000");
                                xml.WriteAttributeString("y2", "530.000000");
                            xml.WriteEndElement();
                        xml.WriteEndElement();
                        xml.WriteStartElement("Bandwidth");
                            xml.WriteStartElement("PhysicsCoeff");
                                xml.WriteAttributeString("RPM", "0.000000");
                                xml.WriteAttributeString("Throttle", "0.000000");
                                xml.WriteAttributeString("PosTorque", "0.000000");
                                xml.WriteAttributeString("NegTorque", "0.000000");
                            xml.WriteEndElement();
                            xml.WriteStartElement("ThreePointCurve");
                                xml.WriteAttributeString("x0", "0.000000");
                                xml.WriteAttributeString("y0", "1.009999");
                                xml.WriteAttributeString("x1", "1.000000");
                                xml.WriteAttributeString("y1", "0.200000");
                                xml.WriteAttributeString("x2", "1.000000");
                                xml.WriteAttributeString("y2", "5.000000");
                            xml.WriteEndElement();
                        xml.WriteEndElement();
                    xml.WriteEndElement();
                    xml.WriteStartElement("LoadPEQ");
                        xml.WriteStartElement("PosLoad");
                            xml.WriteAttributeString("Active", "1");
                            xml.WriteStartElement("Gain");
                                xml.WriteStartElement("PhysicsCoeff");
                                    xml.WriteAttributeString("RPM", "0.000000");
                                    xml.WriteAttributeString("Throttle", "1.000000");
                                    xml.WriteAttributeString("PosTorque", "0.000000");
                                    xml.WriteAttributeString("NegTorque", "0.000000");
                                xml.WriteEndElement();
                                xml.WriteStartElement("ThreePointCurve");
                                    xml.WriteAttributeString("x0", "0.000000");
                                    xml.WriteAttributeString("y0", "1.000000");
                                    xml.WriteAttributeString("x1", "0.500000");
                                    xml.WriteAttributeString("y1", "1.400000");
                                    xml.WriteAttributeString("x2", "1.000000");
                                    xml.WriteAttributeString("y2", "1.900000");
                                xml.WriteEndElement();
                            xml.WriteEndElement();
                            xml.WriteStartElement("CenterFrequency");
                                xml.WriteStartElement("PhysicsCoeff");
                                    xml.WriteAttributeString("RPM", "0.000000");
                                    xml.WriteAttributeString("Throttle", "0.000000");
                                    xml.WriteAttributeString("PosTorque", "0.000000");
                                    xml.WriteAttributeString("NegTorque", "0.000000");
                                xml.WriteEndElement();
                                xml.WriteStartElement("ThreePointCurve");
                                    xml.WriteAttributeString("x0", "0.000000");
                                    xml.WriteAttributeString("y0", "1.000000");
                                    xml.WriteAttributeString("x1", "0.500000");
                                    xml.WriteAttributeString("y1", "1.400000");
                                    xml.WriteAttributeString("x2", "1.000000");
                                    xml.WriteAttributeString("y2", "1.900000");
                                xml.WriteEndElement();
                            xml.WriteEndElement();
                            xml.WriteStartElement("Bandwidth");
                                xml.WriteStartElement("PhysicsCoeff");
                                    xml.WriteAttributeString("RPM", "0.000000");
                                    xml.WriteAttributeString("Throttle", "1.000000");
                                    xml.WriteAttributeString("PosTorque", "0.000000");
                                    xml.WriteAttributeString("NegTorque", "0.000000");
                                xml.WriteEndElement();
                                xml.WriteStartElement("ThreePointCurve");
                                    xml.WriteAttributeString("x0", "0.000000");
                                    xml.WriteAttributeString("y0", "1.000000");
                                    xml.WriteAttributeString("x1", "0.500000");
                                    xml.WriteAttributeString("y1", "1.400000");
                                    xml.WriteAttributeString("x2", "1.000000");
                                    xml.WriteAttributeString("y2", "1.900000");
                                xml.WriteEndElement();
                            xml.WriteEndElement();
                        xml.WriteEndElement();
                        xml.WriteStartElement("NegLoad");
                            xml.WriteAttributeString("Active", "1");
                            xml.WriteStartElement("Gain");
                                xml.WriteStartElement("PhysicsCoeff");
                                    xml.WriteAttributeString("RPM", "0.000000");
                                    xml.WriteAttributeString("Throttle", "0.000000");
                                    xml.WriteAttributeString("PosTorque", "0.000000");
                                    xml.WriteAttributeString("NegTorque", "1.000000");
                                xml.WriteEndElement();
                                xml.WriteStartElement("ThreePointCurve");
                                    xml.WriteAttributeString("x0", "0.000000");
                                    xml.WriteAttributeString("y0", "1.250000");
                                    xml.WriteAttributeString("x1", "0.270000");
                                    xml.WriteAttributeString("y1", "1.650000");
                                    xml.WriteAttributeString("x2", "1.000000");
                                    xml.WriteAttributeString("y2", "2.000000");
                                xml.WriteEndElement();
                            xml.WriteEndElement();
                            xml.WriteStartElement("CenterFrequency");
                                xml.WriteStartElement("PhysicsCoeff");
                                    xml.WriteAttributeString("RPM", "0.000000");
                                    xml.WriteAttributeString("Throttle", "0.000000");
                                    xml.WriteAttributeString("PosTorque", "0.000000");
                                    xml.WriteAttributeString("NegTorque", "0.000000");
                                xml.WriteEndElement();
                                xml.WriteStartElement("ThreePointCurve");
                                    xml.WriteAttributeString("x0", "0.000000");
                                    xml.WriteAttributeString("y0", "309.000000");
                                    xml.WriteAttributeString("x1", "0.000000");
                                    xml.WriteAttributeString("y1", "20.000000");
                                    xml.WriteAttributeString("x2", "0.000000");
                                    xml.WriteAttributeString("y2", "20.000000");
                                xml.WriteEndElement();
                            xml.WriteEndElement();
                            xml.WriteStartElement("Bandwidth");
                                xml.WriteStartElement("PhysicsCoeff");
                                    xml.WriteAttributeString("RPM", "0.000000");
                                    xml.WriteAttributeString("Throttle", "0.000000");
                                    xml.WriteAttributeString("PosTorque", "0.000000");
                                    xml.WriteAttributeString("NegTorque", "0.000000");
                                xml.WriteEndElement();
                                xml.WriteStartElement("ThreePointCurve");
                                    xml.WriteAttributeString("x0", "0.000000");
                                    xml.WriteAttributeString("y0", "1.140000");
                                    xml.WriteAttributeString("x1", "0.000000");
                                    xml.WriteAttributeString("y1", "0.200000");
                                    xml.WriteAttributeString("x2", "0.000000");
                                    xml.WriteAttributeString("y2", "0.200000");
                                xml.WriteEndElement();
                            xml.WriteEndElement();
                        xml.WriteEndElement();
                    xml.WriteEndElement();
                    xml.WriteStartElement("Lowpass");
                        xml.WriteAttributeString("Active", "1");
                        xml.WriteStartElement("CutoffFrequency");
                            xml.WriteStartElement("PhysicsCoeff");
                            xml.WriteAttributeString("RPM", "0.000000");
                            xml.WriteAttributeString("Throttle", "1.000000");
                            xml.WriteAttributeString("PosTorque", "0.000000");
                            xml.WriteAttributeString("NegTorque", "0.000000");
                        xml.WriteEndElement();
                        xml.WriteStartElement("ThreePointCurve");
                            xml.WriteAttributeString("x0", "0.000000");
                            xml.WriteAttributeString("y0", "10861.000000");
                            xml.WriteAttributeString("x1", "0.510000");
                            xml.WriteAttributeString("y1", "15176.000000");
                            xml.WriteAttributeString("x2", "1.000000");
                            xml.WriteAttributeString("y2", "20429.000000");
                        xml.WriteEndElement();
                        xml.WriteStartElement("Resonance");
                            xml.WriteStartElement("PhysicsCoeff");
                                xml.WriteAttributeString("RPM", "0.000000");
                                xml.WriteAttributeString("Throttle", "0.000000");
                                xml.WriteAttributeString("PosTorque", "0.000000");
                                xml.WriteAttributeString("NegTorque", "0.000000");
                            xml.WriteEndElement();
                            xml.WriteStartElement("ThreePointCurve");
                                xml.WriteAttributeString("x0", "0.000000");
                                xml.WriteAttributeString("y0", "1.000000");
                                xml.WriteAttributeString("x1", "0.500000");
                                xml.WriteAttributeString("y1", "1.000000");
                                xml.WriteAttributeString("x2", "1.000000");
                                xml.WriteAttributeString("y2", "1.000000");
                        xml.WriteEndElement();
                    xml.WriteEndElement();
                xml.WriteEndElement();
                xml.WriteEndElement();
            }
        }

        static void WriteBankLoops(XmlWriter xml, FSBFile fsb)
        {
            int prevIndex = -1;
            int index = 0;
            int nextIndex = 1;
            foreach (FSBEntry entry in fsb.fsbEntries)
            {
                if (int.TryParse(entry.name, out int i))
                {
                    xml.WriteStartElement("Loop");
                    xml.WriteAttributeString("wavebankindex", index.ToString());
                    xml.WriteAttributeString("rpm_min", prevIndex == -1 ? "500" : fsb.fsbEntries[prevIndex].name);
                    xml.WriteAttributeString("rpm_sample", entry.name);
                    xml.WriteAttributeString("rpm_max", nextIndex == fsb.fsbEntries.Count ? "25000" : fsb.fsbEntries[nextIndex].name);
                    xml.WriteAttributeString("volume", "0.750000");
                    xml.WriteAttributeString("pitch", "1.000000");
                    xml.WriteAttributeString("pitchMin", "1.000000");
                    xml.WriteAttributeString("pitchMax", "1.000000");
                    xml.WriteEndElement();
                }

                prevIndex++;
                index++;
                nextIndex++;
            }
        }
    }
}
