﻿using System;
using System.Text;
using Studyzy.IMEWLConverter.Entities;
using Studyzy.IMEWLConverter.Helpers;

namespace Studyzy.IMEWLConverter.IME
{
    /// <summary>
    ///     用户自定义的短语
    /// </summary>
    [ComboBoxShow(ConstantString.USER_PHRASE, ConstantString.USER_PHRASE_C, 110)]
    public class UserDefinePhrase : BaseImport, IWordLibraryExport //, IWordLibraryTextImport
    {
        public UserDefinePhrase()
        {
            PhraseFormat = "{1},{2}={0}"; //默认搜狗自定义短语的格式
        }

        /// <summary>
        ///     短语的格式{0}是短语{1}是编码{2}是排列的位置
        /// </summary>
        public string PhraseFormat { get; set; }

        public override CodeType CodeType
        {
            get { return CodeType.Unknown; }
        }


        public Encoding Encoding { get; set; }

        public string Export(WordLibraryList wlList)
        {
            var sb = new StringBuilder();
            foreach (WordLibrary wordLibrary in wlList)
            {
                sb.Append(ExportLine(wordLibrary));
                sb.Append("\r\n");
            }
            return sb.ToString();
        }

        public string ExportLine(WordLibrary wl)
        {
            return string.Format(PhraseFormat, wl.Word, CollectionHelper.Descartes(wl.Codes)[0], DefaultRank);
        }

        public WordLibraryList ImportText(string text)
        {
            throw new NotImplementedException();
        }

        public WordLibraryList Import(string path)
        {
            throw new NotImplementedException();
        }

        public WordLibraryList ImportLine(string str)
        {
            throw new NotImplementedException();
        }
    }
}