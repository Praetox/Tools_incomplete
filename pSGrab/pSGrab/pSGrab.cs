using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Threading;

namespace pSGrab
{
    public class ScrDDL
    {
        public bool bGet = false; //Download?
        public string sPath = ""; //Script path
        public string sTarg = ""; //Target path
        public ArrayList aSc = new ArrayList(); //Script
        public ArrayList aPr = new ArrayList(); //Params
        public int iSc = 0; //Script cur
        public int iPr = 0; //Params cur
        public ServCF scConf = new ServCF();
    }
    public class ServCF
    {
        public enum FileCut { Ignore, Kill, RetryIgnore, RetryKill };
        public FileCut fcAct = FileCut.Ignore;
        public int fcReLim = 3;

        public int iThreads = 2;
        public int iWait1 = 0;
        public int iWait2 = 0;
        
        public ServCF() { }
        public ServCF(int iThreads)
        {
            this.iThreads = iThreads;
        }
    }
    public class PSG
    {
        public static int iChr = ((int)'z' - (int)'a') + 1;
        public string[] args = new string[0];
        ScrDDL sd = null;

        public bool bDebug = false;
        public int iArg = 0;
        public int giSc = -1;
        public string gsRoot = "";
        public string[] saSc = new string[] { "" };
        public string[] saC = new string[iChr];
        public string[] gsaV = new string[iChr];
        public string[] gsaOV = new string[iChr];
        public string[][] gsaVa = new string[iChr][];
        public string[][] gsaOVa = new string[iChr][];
        public int gsaOVai = 0;
        public ArrayList alUriLst = new ArrayList();
        public ArrayList alFPaths = new ArrayList();
        public int iFileCnt = 0;
        public int iGrabCnt = 0;

        public PSG(ScrDDL sd)
        {
            this.sd = sd;
            this.bDebug = GUI.bDebug;
            args = new string[sd.aPr.Count];
            for (int a = 0; a < args.Length; a++)
                args[a] = (string)sd.aPr[a];
        }
        public string get(string sQuery)
        {
            infln("!!" + sQuery + "> ");
            GUI.DispRsp = GUI.DispRspV;
            while (
                GUI.DispRsp == GUI.DispRspV ||
                GUI.DispRsp == GUI.DispRspW)
                Thread.Sleep(1);
            string sRet = GUI.DispRsp;
            GUI.DispRsp = "";
            inf(sRet);
            return sRet;
        }
        public static void inf(string sInfo)
        {
            GUI.DispQue.Add("++" + sInfo);
        }
        public static void infln(string sInfo)
        {
            GUI.DispQue.Add(sInfo);
        }
        public string sGetArg()
        {
            string sRet = "";
            if (args.Length > iArg)
            {
                sRet = args[iArg];
                iArg++;
            }
            return sRet;
        }

        public void Exec()
        {
            if (sd.sPath == "") sd.sPath = get("Script name");
            if (sd.sPath.StartsWith("!"))
            {
                sd.sPath = sd.sPath.Substring(1);
                bDebug = true;
            }

            // Load script
            if (bDebug) infln("? Loading \"" + sd.sPath + "\"");
            string sAbsPath = GUI.sAppPath + "Scripts/" + sd.sPath;
            saSc = GUI.FileRead(sAbsPath).Replace("\r", "").Split('\n');
            for (int a = 0; a < saSc.Length; a++)
                saSc[a] = saSc[a].TrimStart(' ', '\t');
            if (saSc.Length == 1)
            {
                infln("");
                infln("- The script seems to be empty.");
                infln("  pSGrab cannot continue.");
            }

            // Prepare first-time execution
            string[] saV = new string[iChr];
            string[][] saVa = new string[iChr][];
            for (int a = 0; a < iChr; a++)
            {
                saC[a] = ""; saV[a] = "";
                saVa[a] = new string[] { "" };
            }

            // LET'S DO THIS SHIT, BITCH.
            hScript(0, 0, "", "dl/", saV, saVa, 0);
            hGetNow("GETNOW");

            infln(">>Execution finished");
        }
        public int hScript(int iScOfs, int iDepth, string sSrc, string sSaveTo, string[] saOV, string[][] saOVa, int saOVai)
        {
            string[] saV = new string[iChr];
            string[][] saVa = new string[iChr][];
            for (int a = 0; a < iChr; a++)
            {
                saV[a] = sSrc;
                saVa[a] = new string[] { "" };
            }
            //if (bDebug) {
            //    infln("?!  lv." + iDepth + " - ln." + (iScOfs + 1));
            //}
            for (int iSc = iScOfs; iSc < saSc.Length; iSc++)
            {
                if (bDebug)
                {
                    infln("? lv." + iDepth + " - ln." + (iSc + 1) + " - ");
                }
                if (saSc[iSc].StartsWith("NAVIGATE "))
                {
                    if (bDebug)
                    {
                        inf("navigate ");
                    }
                    string sTmp = saSc[iSc];
                    sTmp = sTmp.Substring(sTmp.IndexOf(" ") + 1);
                    string[] saNav = GUI.Split(sTmp, ", ");
                    if (saNav.Length == 1)
                    {
                        //Single variable or full url
                        if (bDebug)
                        {
                            inf("(1var/scUrl)");
                        }
                        if (saNav[0].Length < 5)
                        {
                            saNav[0] = "[[" + saNav[0] + "]]";
                        }
                        string sUrl = hParseVar(saNav[0]);
                        string sNewSrc = http.nav(sUrl);
                        infln("  Reading url (" + DLMan.sMLen(sUrl, 56) + ")");
                        iSc = hScript(iSc + 1, iDepth + 1, sNewSrc, sSaveTo, saV, saVa, 0);
                    }
                    else
                    {
                        //Iteration through array
                        if (bDebug)
                        {
                            inf("(iterate)");
                        }
                        int iResumeAt = iSc;
                        string[] sURLs = hParseRef(saNav[0]);
                        int iOfs1 = Convert.ToInt32(saNav[1]);
                        int iOfs2 = Convert.ToInt32(saNav[2]);
                        for (int a = iOfs1; a < sURLs.Length - iOfs2; a++)
                        {
                            if (bDebug)
                            {
                                infln("?   it." + (a + 1) + " - url: " + sURLs[a] + " - ");
                            }
                            string sUrl = hParseVar(sURLs[a]);
                            string sNewSrc = http.nav(sUrl);
                            infln("  Reading url " + (a + 1) + "/" + sURLs.Length + " (" + DLMan.sMLen(sUrl, 48) + ")");
                            iResumeAt = hScript(iSc + 1, iDepth + 1, sNewSrc, sSaveTo, saV, saVa, a);
                        }
                        iSc = iResumeAt;
                    }
                }
                else if (saSc[iSc] == "RETURN")
                {
                    if (bDebug)
                    {
                        inf("returning");
                    }
                    return iSc;
                }
                else
                {
                    gsaV = saV;
                    gsaVa = saVa;
                    gsaOV = saOV;
                    gsaOVa = saOVa;
                    gsaOVai = saOVai;
                    gsRoot = sSaveTo;
                    giSc = iSc;
                    hGrab(saSc[iSc]);
                    hGetNow(saSc[iSc]);
                    hSkew(saSc[iSc]);
                    hSplit(saSc[iSc]);
                    hSetVar(saSc[iSc]);
                    hAddCut(saSc[iSc]);
                    hReplace(saSc[iSc]);
                    hChRoot(saSc[iSc]);
                    hEcho(saSc[iSc]);
                    hMath(saSc[iSc]);
                    hGoto(saSc[iSc]);
                    hEvent(saSc[iSc]);
                    iSc = giSc;
                    sSaveTo = gsRoot;
                    saOVai = gsaOVai;
                    saOVa = gsaOVa;
                    saOV = gsaOV;
                    saVa = gsaVa;
                    saV = gsaV;
                }
            }
            return saSc.Length;
        }
        
        public bool hEcho(string sSc)
        {
            if (!sSc.StartsWith("ECHO "))
            {
                return false;
            }
            if (bDebug)
            {
                inf("echo");
            }
            string sAct = sSc.Substring(sSc.IndexOf(" ") + 1);
            char cTarget = sAct[sAct.Length - 1];
            int iTarget = (int)cTarget - (int)'a';
            if (sAct.Length > 1)
            {
                char cType = sAct[sAct.Length - 2];
                if (cType == 'v')
                {
                    infln("  Values of va " + cTarget + ":");
                    EchoAry(gsaVa[iTarget]);
                }
                else
                {
                    infln("? C" + cTarget + ": " + saC[iTarget]);
                }
            }
            else
            {
                infln("? V" + cTarget + ": " + gsaV[iTarget]);
            }
            return true;
        }
        public void EchoAry(string[] sa)
        {
            for (int a = 0; a < sa.Length; a++)
            {
                infln("  " + a + ": " + sa[a]);
            }
        }

        public string hParseVar(string sVar)
        {
            return hParseVar(sVar, -1);
        }
        public string hParseVar(string sVar, int aLoc)
        {
            for (int a = 0; a < iChr; a++)
            {
                char cThis = (char)((int)'a' + a);
                if (sVar.Contains(cThis + "]]"))
                {
                    sVar = sVar.Replace("[[c" + cThis + "]]", saC[a]);
                    sVar = sVar.Replace("[[" + cThis + "]]", gsaV[a]);
                    sVar = sVar.Replace("[[o" + cThis + "]]", gsaOV[a]);
                    sVar = sVar.Replace("[[!ov" + cThis + "]]", gsaOVa[a][gsaOVai]);
                    if (aLoc != -1)
                    {
                        sVar = sVar.Replace("[[v" + cThis + "]]", gsaVa[a][aLoc]);
                        sVar = sVar.Replace("[[ov" + cThis + "]]", gsaOVa[a][aLoc]);
                    }
                }
                if (aLoc == -1)
                {
                    int iMaxElms = Math.Max(gsaVa[a].Length, gsaOVa[a].Length);
                    for (int b = 0; b < iMaxElms; b++)
                    {
                        if (sVar.Contains("-" + b + "]]"))
                        {
                            sVar = sVar.Replace("[[v" + cThis + "-" + b + "]]", gsaVa[a][b]);
                            sVar = sVar.Replace("[[ov" + cThis + "-" + b + "]]", gsaOVa[a][b]);
                        }
                    }
                }
            }
            return sVar;
        }
        public string[] hParseRef(string sVar)
        {
            int iTarget = ((int)sVar[sVar.Length - 1]) - (int)'a';
            if (sVar.Length == 1)
            {
                // ary.v
                return gsaVa[iTarget];
            }
            else if (sVar.Length == 2)
            {
                //ary.ov
                return gsaOVa[iTarget];
            }
            return new string[0];
        }

        public bool hEvent(string sSc)
        {
            if (!sSc.StartsWith("EVENT "))
            {
                return false;
            }
            if (bDebug)
            {
                inf("event ");
            }
            sSc = sSc.Substring(sSc.IndexOf(" ") + 1);
            string[] sAct = GUI.Split(sSc, " ");
            if (sAct[0] == "file_cut")
            {
                if (sAct[1] == "retry.kill")
                {
                    sd.scConf.fcAct = ServCF.FileCut.RetryKill;
                    sd.scConf.fcReLim = Convert.ToInt32(sAct[2]);
                    if (bDebug)
                    {
                        inf("filecut:retry.kill:" + sd.scConf.fcReLim);
                    }
                }
                if (sAct[1] == "retry.ignore")
                {
                    sd.scConf.fcAct = ServCF.FileCut.RetryIgnore;
                    sd.scConf.fcReLim = Convert.ToInt32(sAct[2]);
                    if (bDebug)
                    {
                        inf("filecut:retry.ignore:" + sd.scConf.fcReLim);
                    }
                }
                if (sAct[1] == "kill")
                {
                    sd.scConf.fcAct = ServCF.FileCut.Kill;
                    if (bDebug)
                    {
                        inf("filecut:kill");
                    }
                }
                if (sAct[1] == "ignore")
                {
                    sd.scConf.fcAct = ServCF.FileCut.Ignore;
                    if (bDebug)
                    {
                        inf("filecut:ignore");
                    }
                }
            }
            return true;
        }
        public bool hSetVar(string sSc)
        {
            int iTyp = -1;
            if (sSc.StartsWith("SETC "))
            {
                iTyp = 1;
            }
            else if (sSc.StartsWith("SETV "))
            {
                iTyp = 2;
            }
            else
            {
                return false;
            }
            if (bDebug)
            {
                inf("setvar ");
            }
            string sCval = "", sCpre = "";
            char cCofs = sSc[sSc.IndexOf(" ") + 1];
            if (sSc.Length > "SETX x".Length)
            {
                //Fixed constant OR request with comment
                sCpre = sSc.Substring("SETX x".Length).Substring(0, 2);
            }
            if (sCpre == ", ")
            {
                //Constant with set value (SETC x, val)
                sCval = sSc.Substring(sSc.IndexOf(", ") + 2);
            }
            else
            {
                //Constant request
                sCval = sGetArg();
                if (sCval == "")
                {
                    //...not in args
                    if (sCpre == " /")
                    {
                        //...with comment
                        string sCtip = "";
                        sCtip = sSc.Substring(sSc.IndexOf("//") + 2);
                        infln("  >> " + sCtip);
                    }
                    sCval = get("Value " + cCofs);
                }
            }
            if (bDebug)
            {
                inf(iTyp + "/" + cCofs + " to " + sCval);
            }
            sCval = hParseVar(sCval);
            if (iTyp == 1)
            {
                //Constant
                saC[(int)cCofs - (int)'a'] = sCval;
            }
            if (iTyp == 2)
            {
                //Variable
                gsaV[(int)cCofs - (int)'a'] = sCval;
                gsaVa = new string[iChr][];
                for (int a = 0; a < gsaVa.Length; a++)
                    gsaVa[a] = new string[0];
            }
            return true;
        }
        public bool hGoto(string sSc)
        {
            if (!sSc.StartsWith("GOTO "))
            {
                return false;
            }
            if (bDebug)
            {
                inf("goto #");
            }
            sSc = sSc.Substring(sSc.IndexOf(" ") + 1);
            //zero-based, and for-loop adds one
            giSc = Convert.ToInt32(sSc) - 2;
            if (bDebug)
            {
                inf((giSc + 2) + "");
            }
            return true;
        }
        public bool hReplace(string sSc)
        {
            if (!sSc.StartsWith("REPL ")) return false;
            if (bDebug) inf("replace ");

            sSc = sSc.Substring(sSc.IndexOf(" ") + 1);
            char cTarget = sSc[0];
            int iTarget = (int)cTarget - (int)'a';
            sSc = sSc.Substring(3);
            string sSplitBy = sSc[0] + " ";
            sSc = sSc.Substring(3);
            string sFind = sSc.Substring(0, sSc.IndexOf(sSplitBy));
            string sRepl = sSc.Substring(sSc.IndexOf(sSplitBy) + 2);
            string csFind = hParseVar(sFind);
            string csRepl = hParseVar(sRepl);
            if (bDebug)
            {
                inf(" in " + cTarget + ", \"" + csFind + "\" with \"" + csRepl + "\"");
            }

            for (int a = 0; a < gsaVa[iTarget].Length; a++)
            {
                if (gsaVa[iTarget].Length == 1)
                    if (gsaVa[iTarget][0].Length == 0)
                        break;

                try { gsaVa[iTarget][a] = gsaVa[iTarget][a].Replace(csFind, csRepl); }
                catch { }
            }
            try { gsaV[iTarget] = gsaV[iTarget].Replace(csFind, csRepl); }
            catch { }
            return true;
        }
        public bool hMath(string sSc)
        {
            if (!sSc.StartsWith("MATH ")) return false;
            if (bDebug) inf("math ");

            sSc = sSc.Substring(sSc.IndexOf(" ") + 1);
            int iTargetU = sSc[sSc.IndexOf(", ") - 1];
            iTargetU -= (int)'a';
            bool bIsConstant = false;
            if (sSc.IndexOf(", ") > 1)
            {
                bIsConstant = true;
            }
            sSc = sSc.Substring(sSc.IndexOf(", ") + 2);
            string sVal1 = hParseVar(sSc.Substring(0, sSc.IndexOf(" ")));
            double iVal1 = Convert.ToDouble(sVal1);
            sSc = sSc.Substring(sSc.IndexOf(" ") + 1);
            string sOperator = sSc.Substring(0, sSc.IndexOf(" ")).ToLower();
            sSc = sSc.Substring(sSc.IndexOf(" ") + 1);
            double iVal2 = Convert.ToDouble(hParseVar(sSc));

            double iRet = 0;
            string sAct = "";
            if (sOperator == "+")
            {
                sAct = (iVal1 + "+" + iVal2);
                iRet = iVal1 + iVal2;
            }
            if (sOperator == "-")
            {
                sAct = (iVal1 + "-" + iVal2);
                iRet = iVal1 - iVal2;
            }
            if (sOperator == "*")
            {
                sAct = (iVal1 + "*" + iVal2);
                iRet = iVal1 * iVal2;
            }
            if (sOperator == "/")
            {
                sAct = (iVal1 + "/" + iVal2);
                iRet = iVal1 / iVal2;
            }
            if (sOperator == "%")
            {
                sAct = (iVal1 + "%" + iVal2);
                iRet = iVal1 % iVal2;
            }
            if (sOperator == "pow")
            {
                sAct = (iVal1 + "pow" + iVal2);
                iRet = Math.Pow(iVal1, iVal2);
            }
            if (sOperator == "min")
            {
                sAct = (iVal1 + "min" + iVal2);
                iRet = Math.Min(iVal1, iVal2);
            }
            if (sOperator == "max")
            {
                sAct = (iVal1 + "max" + iVal2);
                iRet = Math.Max(iVal1, iVal2);
            }
            char cTarget = '\0';

            if (bIsConstant)
            {
                cTarget = 'C';
                saC[iTargetU] = "" + (int)Math.Round(iRet);
            }
            else
            {
                cTarget = 'V';
                gsaV[iTargetU] = "" + (int)Math.Round(iRet);
            }
            if (bDebug)
            {
                inf(sAct + " to " + cTarget + (char)(iTargetU + 'a'));
            }
            return true;
        }
        public bool hAddCut(string sSc)
        {
            int iAct = -1;
            if (sSc.StartsWith("REMA ")) iAct = 1;
            else if (sSc.StartsWith("REMB ")) iAct = 2;
            else if (sSc.StartsWith("ADDA ")) iAct = 3;
            else if (sSc.StartsWith("ADDB ")) iAct = 4;
            else return false;

            if (bDebug)
            {
                inf("trim md." + iAct);
            }
            string sAct = hParseVar(sSc.Substring(sSc.IndexOf(", ") + 2));
            char cTarget = sSc[sSc.IndexOf(" ") + 1];
            int iTarget = (int)cTarget - (int)'a';
            string sTmp = "";
            for (int a = 0; a < gsaVa[iTarget].Length; a++)
            {
                if (gsaVa[iTarget].Length == 1)
                    if (gsaVa[iTarget][0].Length == 0)
                        break;

                sTmp = gsaVa[iTarget][a];
                try
                {
                    if (iAct == 1)
                    {
                        sTmp = sTmp.Substring(sTmp.IndexOf(sAct) + sAct.Length);
                    }
                    else if (iAct == 2)
                    {
                        sTmp = sTmp.Substring(0, sTmp.IndexOf(sAct));
                    }
                    else if (iAct == 3)
                    {
                        sTmp = sAct + sTmp;
                    }
                    else if (iAct == 4)
                    {
                        sTmp = sTmp + sAct;
                    }
                    gsaVa[iTarget][a] = sTmp;
                }
                catch { }
            }
            sTmp = gsaV[iTarget];
            try
            {
                if (iAct == 1)
                {
                    sTmp = sTmp.Substring(sTmp.IndexOf(sAct) + sAct.Length);
                }
                else if (iAct == 2)
                {
                    sTmp = sTmp.Substring(0, sTmp.IndexOf(sAct));
                }
                else if (iAct == 3)
                {
                    sTmp = sAct + sTmp;
                }
                else if (iAct == 4)
                {
                    sTmp = sTmp + sAct;
                }
                gsaV[iTarget] = sTmp;
            }
            catch { }
            return true;
        }
        public bool hSkew(string sSc)
        {
            if (!sSc.StartsWith("SKEW ")) return false;
            if (bDebug) inf("skew ");

            int iSteps = Convert.ToInt32(sSc.Substring(sSc.IndexOf(", ") + 2));
            char cTarget = sSc[sSc.IndexOf(" ") + 1];
            int iTarget = (int)cTarget - (int)'a';
            if (bDebug)
            {
                inf(cTarget + " by " + iSteps);
            }
            string[] saRet = new string[gsaVa[iTarget].Length + iSteps];
            for (int a = 0; a < saRet.Length; a++)
            {
                int iLoc = a - iSteps;
                if (iLoc >= 0 && iLoc < gsaVa[iTarget].Length)
                    saRet[a] = gsaVa[iTarget][iLoc];
                else saRet[a] = "";
            }
            gsaVa[iTarget] = saRet;
            return true;
        }
        public bool hSplit(string sSc)
        {
            if (!sSc.StartsWith("SPLIT ")) return false;
            if (bDebug) inf("split ");

            string sAct = hParseVar(sSc.Substring(sSc.IndexOf(", ") + 2));
            char cTarget = sSc[sSc.IndexOf(" ") + 1];
            int iTarget = (int)cTarget - (int)'a';
            gsaVa[iTarget] = GUI.Split(gsaV[iTarget], sAct);
            if (bDebug)
            {
                inf(cTarget + "");
            }
            return true;
        }
        public bool hChRoot(string sSc)
        {
            if (!sSc.StartsWith("SAVETO ")) return false;
            if (bDebug) inf("saveto ");

            string sSub = sSc.Substring(sSc.IndexOf(" ") + 1);
            sSub = hParseVar(sSub.Replace("\\", "/"));
            if (sSub.StartsWith("+"))
            {
                sSub = sSub.Substring(1);
                gsRoot += sSub;
            }
            else
            {
                gsRoot = sSub;
            }
            gsRoot = gsRoot.Trim('/') + "/";
            if (bDebug)
            {
                inf(gsRoot);
            }
            return true;
        }
        public bool hGrab(string sSc)
        {
            if (!sSc.StartsWith("GRAB ")) return false;
            if (bDebug) inf("grab ");

            iGrabCnt++;
            int iThisCnt = 0;
            sSc = sSc.Substring(sSc.IndexOf(" ") + 1);
            int iTargetU = ((int)sSc[0]) - (int)'a';
            sSc = sSc.Substring(sSc.IndexOf(", ") + 2);
            int iOfs1 = Convert.ToInt32(sSc.Substring(0, sSc.IndexOf(", ")));
            sSc = sSc.Substring(sSc.IndexOf(", ") + 2);
            int iOfs2 = Convert.ToInt32(sSc.Substring(0, sSc.IndexOf(", ")));
            sSc = sSc.Substring(sSc.IndexOf(", ") + 2);

            for (int a = iOfs1; a < gsaVa[iTargetU].Length - iOfs2; a++)
            {
                iFileCnt++;
                iThisCnt++;
                string sFn = sSc;
                string sUri = gsaVa[iTargetU][a];
                if (sFn.StartsWith("[[ofn"))
                {
                    string[] sUriParts = GUI.Split(sUri, "/");
                    string sUPCnt = sFn.Substring("[[ofn".Length);
                    sUPCnt = sUPCnt.Substring(0, sUPCnt.IndexOf("]]"));
                    int iUPCnt = Convert.ToInt32(sUPCnt);
                    int iUPTot = sUriParts.Length;
                    sFn = sUriParts[iUPTot - 1];
                    for (int b = 1; b < iUPCnt; b++)
                    {
                        sFn = sUriParts[iUPTot - b - 1] + "\\" + sFn;
                    }
                }
                else
                {
                    for (int b = 0; b < iChr; b++)
                    {
                        char cThis = (char)((int)'a' + b);
                        sFn = sFn.Replace("%g", "" + iGrabCnt);
                        sFn = sFn.Replace("%f", "" + iFileCnt);
                        sFn = sFn.Replace("%i", "" + a);
                        if (sFn.Contains(cThis + "]]"))
                        {
                            sFn = sFn.Replace("[[c" + cThis + "]]", saC[b]);
                            sFn = sFn.Replace("[[" + cThis + "]]", gsaV[b]);
                            sFn = sFn.Replace("[[o" + cThis + "]]", gsaOV[b]);
                            if (gsaVa[b].Length > a)
                            {
                                sFn = sFn.Replace("[[v" + cThis + "]]", gsaVa[b][a]);
                            }
                            if (gsaOVa[b].Length > a)
                            {
                                sFn = sFn.Replace("[[ov" + cThis + "]]", gsaOVa[b][a]);
                            }
                        }
                    }
                }
                //alUriLst.Add(sUri);
                //alFPaths.Add(sd.sPath + gsRoot + sFn);
                FileDL fDL = new FileDL(sUri, GUI.sAppPath + gsRoot + sFn);
                fDL.scConf = sd.scConf; DLMan.Add(fDL);
            }
            infln("- " + "Added " + iThisCnt + " files (" + iFileCnt + " in total)");
            return true;
        }
        public bool hGetNow(string sSc)
        {
            if (!sSc.StartsWith("GETNOW")) return false;
            if (bDebug)
            {
                inf("get now");
            }
            infln("");
            DLMan.Grab();
            return true;
        }
        /*public bool hGetNowST(string sSc)
        {
            if (!sSc.StartsWith("GETNOW")) return false;
            if (bDebug)
            {
                inf("get now");
            }
            string[] saPaths = new string[alFPaths.Count];
            string[] saFUris = new string[alUriLst.Count];
            for (int a = 0; a < saPaths.Length; a++) saPaths[a] = (string)alFPaths[a];
            for (int a = 0; a < saFUris.Length; a++) saFUris[a] = (string)alUriLst[a];
            alFPaths.Clear(); alUriLst.Clear();
            int iUriCnt = saFUris.Length;
            if (saFUris[0].Length == 0)
            {
                iUriCnt = 0;
            }
            int iRetryCount = 0;
            infln("");
            infln(string.Format("  {0,-40} [--------------------]", ">> " + iUriCnt + " files to download"));

            for (int a = 0; a < iUriCnt; a++)
            {
                saPaths[a] = sFilterPath(saPaths[a].Replace("\\", "/"));
                string sFName = saPaths[a].Substring(saPaths[a].LastIndexOf("/") + 1);
                infln(string.Format("  {0,-40} [", sMLen((a + 1) + ": " + sFName, 40)));
                if (System.IO.File.Exists(saPaths[a]))
                {
                    inf("File exists         ]");
                }
                else
                {
                    int[] iGRet = http.get(saFUris[a], saPaths[a], 20);
                    inf("] (" + (iGRet[1] / 1024) + "KB)");
                    if (iGRet[1] < iGRet[0] && iGRet[1] >= 0)
                    {
                        infln("- " + iGRet[1] + "<" + iGRet[0]);
                        if (iCutFile_action == 0)
                        { //ignore

                            iCutFile_ignore++;
                            infln("- File cut - ignore error #" + iCutFile_ignore);
                        }
                        if (iCutFile_action == 1)
                        { //kill

                            iCutFile_skip++;
                            System.IO.File.Delete(saPaths[a]);
                            infln("- File cut - kill and resume #" + iCutFile_skip);
                        }
                        if (iCutFile_action >= 2)
                        { //retry

                            if (iRetryCount < iCutFile_reLim)
                            {
                                iRetryCount++;
                                System.IO.File.Delete(saPaths[a]); a--;
                                infln("- File cut - retry #" + iRetryCount);
                            }
                            else
                            {
                                if (iCutFile_action == 2)
                                { //retry-ignore

                                    iCutFile_ignore++;
                                    infln("- File cut - ignore error #" + iCutFile_ignore);
                                    iRetryCount = 0;
                                }
                                else
                                {
                                    iCutFile_skip++;
                                    System.IO.File.Delete(saPaths[a]);
                                    infln("- File cut - kill and resume #" + iCutFile_skip);
                                    iRetryCount = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        iRetryCount = 0;
                    }
                }
            }
            infln("");
            return true;
        }*/
    }
    public class http
    {
        public static string nav(string sUrl)
        {
            WebGet wg = new WebGet(sUrl, "");
            while (wg.State != WebGet.eState.Completed
                && wg.State != WebGet.eState.Failed)
                System.Threading.Thread.Sleep(1);
            return wg.sRBody;
        }
        public static int[] get(string sUrl, string sFName, int iSpan)
        {
            WebGet wg = new WebGet(sUrl, "", GUI.sAppPath + sFName);
            while (wg.iLen == 0)
                System.Threading.Thread.Sleep(1);

            int iPrCnt = 0;
            double dPrStp = (double)wg.iLen / (double)iSpan;
            while (true)
            {
                if (dPrStp > 0.01)
                {
                    int iPrInc = (int)Math.Round((double)wg.iRecv / dPrStp) - iPrCnt;
                    if (iPrInc > 0)
                    {
                        iPrCnt += iPrInc;
                        string sPrInc = "";
                        for (int a = 0; a < iPrInc; a++)
                        {
                            sPrInc += "*";
                        }
                        PSG.inf(sPrInc);
                    }
                }
                if (wg.State == WebGet.eState.Completed ||
                    wg.State == WebGet.eState.Failed) break;
                System.Threading.Thread.Sleep(10);
            }
            if (wg.State == WebGet.eState.Failed)
            {
                PSG.inf("error");
                return new int[] { -2, -1 };
            }
            int iPad = iSpan - iPrCnt;
            if (iSpan != 0 && iPad > 0)
            {
                PSG.inf(new string(' ', iPad));
            }
            return new int[] { wg.iLen, wg.iRecv };
        }
    }
    public class WebGet
    {
        public Uri uUrl;
        public string sPath;
        public int iRecv;
        public int iLen;
        public string sRHead;
        public string sRBody;
        public eState State;
        public FileDL Info = new FileDL();
        System.ComponentModel.BackgroundWorker bw =
            new System.ComponentModel.BackgroundWorker();
        public enum eState { Ready, Connecting, Downloading, Completed, Failed };

        public WebGet(FileDL fDL)
        {
            Info = fDL;
            iRecv = 0; iLen = 0;
            uUrl = new Uri(Info.sFUri);
            sPath = (Info.sFPath + Info.sFName).Replace("\\", "/");
            sRHead = ""; sRBody = ""; State = eState.Connecting;
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(GetFile);
            bw.RunWorkerAsync();
        }
        public WebGet(string sUrl, string sAuth)
        {
            uUrl = new Uri(sUrl);
            sPath = "";
            sRHead = ""; sRBody = "";
            iRecv = 0; iLen = 0;
            State = eState.Connecting;
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(GetString);
            bw.RunWorkerAsync();
        }
        public WebGet(string sUrl, string sAuth, string sFilePath)
        {
            uUrl = new Uri(sUrl);
            sPath = sFilePath.Replace("\\", "/");
            sRHead = ""; sRBody = "";
            iRecv = 0; iLen = 0;
            State = eState.Connecting;
            bw.DoWork += new System.ComponentModel.DoWorkEventHandler(GetFile);
            bw.RunWorkerAsync();
        }
        void GetString(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            object[] wrObj = wrStream();
            if (wrObj.Length == 0) return;
            Stream wrData = (Stream)wrObj[1];
            WebHeaderCollection wrHead = (WebHeaderCollection)wrObj[0];
            if (wrHead.GetValues("Content-Length") == null) iLen = 0;
            else iLen = Convert.ToInt32(wrHead.GetValues("Content-Length")[0]);

            MemoryStream ms = new MemoryStream();
            while (true)
            {
                byte[] b = new byte[8192];
                int ib = wrData.Read(b, 0, b.Length);
                if (ib == 0) break;
                iRecv += ib;
                ms.Write(b, 0, ib);
            }
            sRBody = System.Text.Encoding.UTF8.GetString(ms.GetBuffer());
            ms.Close(); ms.Dispose(); wrData.Close(); wrData.Dispose();
            State = eState.Completed;
        }
        void GetFile(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            object[] wrObj = wrStream();
            if (wrObj.Length == 0) return;
            Stream wrData = (Stream)wrObj[1];
            WebHeaderCollection wrHead = (WebHeaderCollection)wrObj[0];
            if (wrHead.GetValues("Content-Length") == null) iLen = 0;
            else iLen = Convert.ToInt32(wrHead.GetValues("Content-Length")[0]);

            PadFile(sPath, iLen);
            FileStream fs = new FileStream(sPath,
                FileMode.Open, FileAccess.Write);
            fs.Seek(0, SeekOrigin.Begin);
            while (true)
            {
                byte[] b = new byte[8192];
                int ib = wrData.Read(b, 0, b.Length);
                if (ib == 0) break;
                iRecv += ib;
                fs.Write(b, 0, ib);
            }
            fs.Flush(); fs.Close(); fs.Dispose();
            wrData.Close(); wrData.Dispose();
            State = eState.Completed;
        }

        private object[] wrStream()
        {
            HttpWebRequest wReq = (HttpWebRequest)HttpWebRequest.Create(uUrl);
            wReq.UserAgent = "pSGrab/0.1";
            if (uUrl.UserInfo != "")
                wReq.Headers.Add("Authorization", "Basic " + Convert.
                    ToBase64String(Encoding.ASCII.GetBytes(uUrl.UserInfo)));
            try
            {
                HttpWebResponse wRsp = (HttpWebResponse)wReq.GetResponse();
                State = eState.Downloading;
                return new object[] {
                    wRsp.Headers,
                    wRsp.GetResponseStream() };
            }
            catch
            {
                State = eState.Failed;
                return new object[] { };
            }
        }
        public static void MkPath(string sPath)
        {
            sPath = sPath.Substring(0, sPath.LastIndexOf("/"));
            if (!sPath.Contains("/")) return;
            if (!System.IO.Directory.Exists(sPath))
                System.IO.Directory.CreateDirectory(sPath);
        }
        public static void PadFile(string sPath, long iSize)
        {
            MkPath(sPath);
            FileStream fs = new FileStream(sPath, FileMode.Create);
            byte[][] b = new byte[7][] {
                new byte[1000000],
                new byte[100000],
                new byte[10000],
                new byte[1000],
                new byte[100],
                new byte[10],
                new byte[1] };
            for (int a = 0; a < b.Length; a++)
            {
                while (fs.Length < iSize &&
                    fs.Length + b[a].Length <= iSize)
                    fs.Write(b[a], 0, b[a].Length);
            }
            fs.Close(); fs.Dispose();
        }
    }

    public class FileDL
    {
        public string sFUri = "";
        public string sFName = "";
        public string sFPath = "";
        public ServCF scConf = new ServCF();
        public int fcReCnt = 0;
        public FileDL() { }
        public FileDL(string sUri)
        {
            sFUri = sUri;
            sFName = DLMan.sFilterName(sFUri.Substring(sFUri.LastIndexOf("/")+1));
        }
        public FileDL(string sUri, string sPath)
        {
            sFUri = sUri; sPath = sPath.Replace("\\", "/");
            sFPath = DLMan.sFilterPath(sPath.Substring(0, sPath.LastIndexOf("/")+1));
            sFName = DLMan.sFilterName(sPath.Substring(sPath.LastIndexOf("/")+1));
        }
        public FileDL(string sUri, string sPath, string sName)
        {
            sFUri = sUri;
            sFPath = DLMan.sFilterPath(sPath.Replace("\\", "/"));
            sFName = DLMan.sFilterName(sName);
        }
    }
    public class DLMan
    {
        public static char[] badFileChr = new char[] { ':', '*', '?', '\"', '<', '>', '|', '\\', '/' };
        public static char[] badPathChr = new char[] { ':', '*', '?', '\"', '<', '>', '|' };
        public static ArrayList alQue = new ArrayList(); //FileDL
        public static WebGet[] Cli = new WebGet[10];
        public static bool bGrab = false;
        public static int iCutFile_ignore = 0;
        public static int iCutFile_retry = 0;
        public static int iCutFile_skip = 0;

        public static void Add(FileDL fd)
        {
            alQue.Add(fd);
        }
        public static void Grab()
        {
            PSG.infln("");
            PSG.infln(string.Format("  {0,-40} [--------------------]", ">> " + alQue.Count + " files to download"));
            while (alQue.Count > 0)
            {
                tryGrab();
                System.Threading.Thread.Sleep(1);
            }
        }
        public static void tryGrab()
        {
            if (bGrab) return; bGrab = true;
            if (!isDowning() && isGood()
                && alQue.Count == 0)
            {
                bGrab = false; return;
            }
            for (int a = 0; a < Cli.Length; a++)
            {
                if (Cli[a] == null ||
                    Cli[a].State == WebGet.eState.Completed ||
                    Cli[a].State == WebGet.eState.Ready)
                {
                    if (alQue.Count == 0) break;
                    FileDL fDL = (FileDL)alQue[0];
                    if (a >= fDL.scConf.iThreads) break;
                    alQue.RemoveAt(0);

                    PSG.infln(string.Format("  {0,-40} [", sMLen((a + 1) + ": " + fDL.sFName, 40)));
                    if (System.IO.File.Exists(fDL.sFPath + fDL.sFName))
                    {
                        PSG.inf("Exists");
                    }
                    else
                    {
                        PSG.inf("Downloading");
                        Cli[a] = new WebGet(fDL);
                    }
                }
                if (Cli[a] != null &&
                    Cli[a].State == WebGet.eState.Failed)
                {
                    string sOut = string.Format(" {0,-40} [", sMLen((a + 1) + ": " + Cli[a].Info.sFName, 40));
                    string sAbsPath = Cli[a].Info.sFPath + Cli[a].Info.sFName;
                    if (Cli[a].Info.scConf.fcAct == ServCF.FileCut.Ignore)
                    {
                        sOut = "/" + sOut; iCutFile_ignore++;
                        sOut += "DC - Ignore #" + iCutFile_ignore;
                        Cli[a].State = WebGet.eState.Ready;
                        PSG.infln(sOut);
                    }
                    if (Cli[a].Info.scConf.fcAct == ServCF.FileCut.Kill)
                    {
                        sOut = "X" + sOut; iCutFile_skip++;
                        System.IO.File.Delete(sAbsPath);
                        sOut += "DC - Delete #" + iCutFile_skip;
                        Cli[a].State = WebGet.eState.Ready;
                        PSG.infln(sOut);
                    }
                    if (Cli[a].Info.scConf.fcAct == ServCF.FileCut.RetryIgnore ||
                        Cli[a].Info.scConf.fcAct == ServCF.FileCut.RetryKill)
                    {
                        if (Cli[a].Info.fcReCnt < Cli[a].Info.scConf.fcReLim)
                        {
                            sOut = "~" + sOut; Cli[a].Info.fcReCnt++;
                            System.IO.File.Delete(sAbsPath);
                            sOut += "DC - Retry #" + Cli[a].Info.fcReCnt;
                            Cli[a] = new WebGet(Cli[a].Info);
                            PSG.infln(sOut);
                        }
                        else
                        {
                            if (Cli[a].Info.scConf.fcAct == ServCF.FileCut.RetryIgnore)
                            {
                                sOut = "/" + sOut; iCutFile_ignore++;
                                sOut += "DC - Ignore #" + iCutFile_ignore;
                                Cli[a].State = WebGet.eState.Ready;
                                PSG.infln(sOut);
                            }
                            else
                            {
                                sOut = "X" + sOut; iCutFile_skip++;
                                System.IO.File.Delete(sAbsPath);
                                sOut += "DC - Delete #" + iCutFile_skip;
                                Cli[a].State = WebGet.eState.Ready;
                                PSG.infln(sOut);
                            }
                        }
                    }
                }
            }
            bGrab = false;
        }
        public static bool isDowning()
        {
            bool bRet = false;
            for (int a = 0; a < Cli.Length; a++)
                if (Cli[a] != null)
                    if (Cli[a].State == WebGet.eState.Connecting ||
                        Cli[a].State == WebGet.eState.Downloading)
                        bRet = true;
            return bRet;
        }
        public static bool isGood()
        {
            bool bRet = true;
            for (int a = 0; a < Cli.Length; a++)
                if (Cli[a] != null)
                    if (Cli[a].State == WebGet.eState.Completed)
                        if (Cli[a].iRecv < Cli[a].iLen)
                        {
                            Cli[a].State = WebGet.eState.Failed;
                            bRet = false;
                        }
            return bRet;
        }
        public static string sFilterPath(string sPath)
        {
            for (int a = 0; a < badPathChr.Length; a++)
            {
                sPath = sPath.Substring(0, 3) + sPath.Substring(3)
                    .Replace("" + badPathChr[a], "_");
            }
            return sPath;
        }
        public static string sFilterName(string sName)
        {
            for (int a = 0; a < badPathChr.Length; a++)
            {
                sName = sName.Replace("" + badPathChr[a], "_");
            }
            return sName;
        }
        public static string sMLen(string str, int len)
        {
            if (str.Length <= len)
            {
                return str;
            }
            str = str.Substring(0, len - 3);
            return str + "...";
        }
    }
}
