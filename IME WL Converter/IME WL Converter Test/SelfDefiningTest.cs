﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;
using Studyzy.IMEWLConverter.Entities;
using Studyzy.IMEWLConverter.IME;

namespace Studyzy.IMEWLConverter.Test
{
    [TestFixture]
    public class SelfDefiningTest
    {
        private SelfDefining selfDefining;
        [TestFixtureSetUp]
        public void Init()
        {
            selfDefining=new SelfDefining();
        }
        [Test]
        public void TestPinyinString2WL()
        {
            ParsePattern parser = new ParsePattern()
            {
                IsPinyinFormat = true,
                CodeSplitType = BuildType.FullContain,
                CodeSplitString = ",",
                ContainCode = true,
                ContainRank = true,
                SplitString = " ",
                Sort = new List<int>() { 2, 1, 3 }
            };
            var str = "深蓝 ,shen,lan, 1";
            selfDefining.UserDefiningPattern = parser;
            var wl = selfDefining.ImportLine(str)[0];

            Assert.AreEqual(wl.Codes[0][0], "shen");
            Assert.AreEqual(wl.Codes[1][0], "lan");
            Assert.AreEqual(wl.Rank, 1);
        }

        [Test]
        public void TestWordLibrary2String()
        {
            ParsePattern parser = new ParsePattern()
            {
                IsPinyinFormat = true,
                CodeSplitType = BuildType.FullContain,
                CodeSplitString = ",",
                ContainCode = true,
                ContainRank = true,
                SplitString = "|",
                CodeType = CodeType.Pinyin,
                Sort = new List<int>() { 2, 1, 3 }
            };
            WordLibrary wl = new WordLibrary() { Word = "深蓝", Rank = 123, CodeType = CodeType.Pinyin };
            wl.Codes = new IList<string>[2];
            wl.Codes[0] = new[] { "shen" };
            wl.Codes[1] = new[] { "lan" };
            selfDefining.UserDefiningPattern = parser;
            var str = selfDefining.ExportLine(wl);
            Assert.AreEqual(str, "深蓝|,shen,lan,|123");
        }
        [Test]
        public void TestGeneratePinyinThen2String()
        {
            ParsePattern parser = new ParsePattern()
            {
                IsPinyinFormat = true,
                CodeSplitType = BuildType.FullContain,
                CodeSplitString = "~",
                ContainCode = true,
                ContainRank = true,
                SplitString = "|",
                CodeType = CodeType.Pinyin,
                LineSplitString = "\r",
                Sort = new List<int>() { 2, 1, 3 }
            };
            WordLibraryList wll = new WordLibraryList();
            WordLibrary wl = new WordLibrary() { Word = "深蓝", Rank = 123, CodeType = CodeType.UserDefine };
            wl.Codes = new IList<string>[2];
            wl.Codes[0] = new[] { "sn" };
            wl.Codes[1] = new[] { "ln" };
            wll.Add(wl);
            selfDefining.UserDefiningPattern = parser;
            var str = selfDefining.Export(wll);
            Assert.AreEqual(str, "深蓝|~shen~lan~|123\r");
        }
        private WordLibrary WlData = new WordLibrary
        {
            Rank = 10, 
            PinYin = new[] { "shen", "lan", "ce", "shi" }, 
            Word = "深蓝测试",
            CodeType = CodeType.Pinyin
        };
       
        [Test]
        public void TestExportPinyinDifferentFormatWL()
        {
            var p = InitPattern();
            p.Sort=new List<int>(){3,2,1};
            p.SplitString = "$";
            p.CodeSplitString = "_";
            p.CodeSplitType= BuildType.None;
            p.IsPinyinFormat = true;
            p.CodeType = CodeType.Pinyin;
            selfDefining.UserDefiningPattern = p;
            var str = selfDefining.Export(new WordLibraryList() { WlData });
            Debug.WriteLine(str);
            Assert.AreEqual(str, "深蓝测试$shen_lan_ce_shi\r\n");
        }
        [Test]
        public void TestExportExtCodeWL()
        {
            selfDefining.UserDefiningPattern = InitPattern();
            selfDefining.UserDefiningPattern.MappingTablePath = "Test\\array30.txt";
            var str = selfDefining.Export(new WordLibraryList() { WlData });
            Debug.WriteLine(str);
            Assert.IsNotNullOrEmpty(str);
        }
        //[Test]
        //public void TestWLWithoutPinyinExportException()
        //{
        //    export.UserDefiningPattern = InitPattern();
        //    var str = export.Export(new WordLibraryList() { new WordLibrary { Count = 10, Word = "深蓝测试" } });
        //    Debug.WriteLine(str);
        //    Assert.IsNullOrEmpty(str);
        //}
        [Test]
        public void TestExportExtCodeLots()
        {
            string str="深蓝词库转换测试代码";
            var list = new WordLibraryList();
            var ts = "";
            foreach (var c in str)
            {
                ts += c;
                list.Add(new WordLibrary() {Rank = 10, IsEnglish = false, Word = ts});
            }


            selfDefining.UserDefiningPattern = InitPattern();
            selfDefining.UserDefiningPattern.MappingTablePath = "Test\\array30.txt";
            var x = selfDefining.Export(list);
            Debug.WriteLine(x);
            Assert.IsNotNullOrEmpty(str);
        }
        private ParsePattern InitPattern()
        {
            ParsePattern pp=new ParsePattern();
            pp.MutiWordCodeFormat = @"code_e2=p11+p12+p21+p22
code_e3=p11+p21+p31+p32
code_a4=p11+p21+p31+n11";
            pp.IsPinyinFormat = false;
            pp.ContainRank = false;
            pp.SplitString = " ";
            pp.ContainCode = true;
            pp.TextEncoding = Encoding.UTF8;
            return pp;
        }
     
    }
}
