//using System;
//using System.Text;
//using System.CodeDom;
//using System.Threading;
//using System.IO;

//using MonoDevelop.Core;
//using MonoDevelop.Ide.Gui;
//using MonoDevelop.Components.Commands;
//using MonoDevelop.Projects;
//using MonoDevelop.Projects.Text;
//using MonoDevelop.Projects.Parser;
//using MonoDevelop.Projects.Ambience;
//using MonoDevelop.Ide.Gui.Content;
//using MonoDevelop.Ide.Gui.Search;
//using MonoDevelop.Projects.CodeGeneration;
//using MonoDevelop.Ide.Gui.Dialogs;

//namespace MonoDevelop.Ide.Commands
//{
//    public class helperClass
//    {

//        public static string ExtractTextFromRegion(IRegion Region, string tempfile)
//        {
//            return helperClass.ExtractRegionFromFile(Region.BeginLine, Region.BeginColumn, Region.EndLine, Region.EndColumn, Region.FileName, tempfile);
//        }
//        public static string GetTextFromRegion(IRegion Region)
//        {
//            return helperClass.GetTextFromRegion(Region.BeginLine, Region.BeginColumn, Region.EndLine, Region.EndColumn, Region.FileName);
//        }
//        public static String ExtractRegionFromFile(int BeginLine, int BeginColumn, int EndLine, int EndColumn, String FileName, String tempfile)
//        {
//            if (BeginColumn < 1 || BeginLine < 1 || EndColumn < 1 || EndLine < BeginLine)
//            {
//                throw new ArgumentException("parameters that define the IRegion(line or column indexes are not valid) are invalid");
//            }
//            if (!System.IO.File.Exists(FileName))
//            {
//                throw new ArgumentException("the file " + FileName + " doesn't exists");
//            }
//            String line = null;
//            System.IO.StreamReader sr = null;
//            System.IO.StreamWriter sw = null;
//            try
//            {
//                sr = new System.IO.StreamReader(FileName);
//            }
//            catch (Exception x)
//            {
//                throw x;
//            }
//            //read all the lines before this region to move at the line with the region
//            //TODO maybe find a more eficient way to do this
//            try
//            {
//                sw = new System.IO.StreamWriter(tempfile);
//            }
//            catch (Exception x)
//            {
//                throw new IOException("canot create file: " + tempfile + "  " + x.Message);
//            }
//            if (BeginLine > 1)
//            {

//                for (Int32 i = 1; i <= BeginLine - 1; i++)
//                {
//                    line = sr.ReadLine();
//                    if (line == null) throw new Exception("error unexpected end of file at line " + i.ToString() + " in fie " + FileName);
//                    sw.WriteLine(line);
//                }
//            }
//            //read the characters from the first line of the region before the begin column position
//            if (BeginColumn > 1)
//            {//sw.WriteLine("the begin column is not 1");

//                char[] text = new char[BeginColumn - 1];
//                sr.Read(text, 0, BeginColumn - 1);

//                sw.Write(text);

//            }

//            //now will get the text from the region
//            System.Text.StringBuilder sb = new System.Text.StringBuilder(1000);//i used a big value to prevent the resize of the String Buillder
//            if (BeginLine == EndLine)
//            {
//                //sw.WriteLine("the region has only 1 line");
//                char[] text = new char[EndColumn - BeginColumn + 1]; ;
//                sr.Read(text, 0, EndColumn - BeginColumn + 1);
//                sb.Append(text);
//                sw.Write(sr.ReadToEnd());
//                sr.Close();
//                sw.Close();
//                return sb.ToString();
//            }
//            else//if  beginline <EndLine
//            { //read the first line from the BeginColumn
//              //sw.Write(Environment.NewLine);
//                line = sr.ReadLine();
//                if (line == null) throw new Exception("unexpected end of file in file : " + FileName + "at line : " + BeginLine.ToString());
//                sb.Append(line + System.Environment.NewLine);

//                //read the rest of the lines except the last one
//                if (EndLine - BeginLine > 2)
//                {//if the region has more then 2 lines

//                    for (Int32 i = BeginLine + 1; i <= EndLine - 1; i++)
//                    {
//                        line = sr.ReadLine(); if (line == null) throw new Exception("unexpected end of file in file : " + FileName + "at line: " + i.ToString());
//                        sb.Append(line + System.Environment.NewLine);
//                    }
//                }
//                //add the last line
//                char[] text = new char[EndColumn];
//                sr.Read(text, 0, EndColumn);
//                sb.Append(text);
//                sb.Append(System.Environment.NewLine);

//                //reading the rest of the file in the temp file
//                sw.Write(sr.ReadToEnd());
//                sw.Close();
//                sr.Close();
//                return sb.ToString();
//            }
//        }
//        public static string DetermineValidFileName(string originalname, string classname)
//        {
//            string dir;
//            dir = Path.GetDirectoryName(originalname);
//            string name;
//            string ext;
//            ext = Path.GetExtension(originalname);
//            name = dir + Path.DirectorySeparatorChar + classname + ext;
//            if (File.Exists(name))
//            {
//                for (Int32 i = 2; i < 30000; i++)
//                {
//                    name = dir + Path.DirectorySeparatorChar + classname + i.ToString() + ext;
//                    if (!File.Exists(name)) return name;
//                }
//                throw new Exception("can't create the filename, can't generate a valid name");
//            }
//            else
//            {
//                return name;
//            }
//        }

//        public static string GetTextFromRegion(int BeginLine, int BeginColumn, int EndLine, int EndColumn, String FileName)
//        {
//            if (BeginColumn < 1 || BeginLine < 1 || EndColumn < 1 || EndLine < BeginLine)
//            {
//                throw new ArgumentException("parameters that define the IRegion(line or column indexes are not valid) are invalid");
//            }
//            if (!System.IO.File.Exists(FileName))
//            {
//                throw new ArgumentException("the file " + FileName + " doesn't exists");
//            }
//            System.IO.StreamReader sr = null;
//            try
//            {
//                sr = new System.IO.StreamReader(FileName);
//            }
//            catch (Exception x)
//            {
//                throw x;
//            }
//            //read all the lines before this region to move at the line with the region
//            //TODO maybe find a more eficient way to do this
//            if (BeginLine > 1)
//            {
//                //sw.WriteLine("the begin line is not the first line");
//                for (Int32 i = 1; i <= BeginLine - 1; i++)
//                {
//                    sr.ReadLine();
//                    //	sw.WriteLine("Read the line numer  "+i.ToString());
//                }
//            }
//            //read the characters from the first line of the region before the begin column position
//            if (BeginColumn > 1)
//            {//sw.WriteLine("the begin column is not 1");
//                for (Int32 i = 1; i <= BeginColumn - 1; i++)
//                {
//                    //sw.WriteLine("Read the  character number  "+i.ToString());
//                    sr.Read();//maybe here i can use an overload that can read more then one character
//                }
//            }

//            //now will get the text from the region
//            System.Text.StringBuilder sb = new System.Text.StringBuilder(1000);//i used a big value to prevent the resize of the String Buillder
//            if (BeginLine == EndLine)
//            {
//                //sw.WriteLine("the region has only 1 line");
//                char[] text = new char[EndColumn - BeginColumn + 1]; ;
//                sr.Read(text, 0, EndColumn - BeginColumn + 1);
//                sb.Append(text);
//                sr.Close();
//                //sw.Close();
//                return sb.ToString();
//            }
//            else//the beginline <EndLine
//            { //read the first line from the BeginColumn
//              //sw.WriteLine("the region has more then 1 line");

//                sb.Append(sr.ReadLine() + System.Environment.NewLine);
//                //sw.WriteLine("read the first line");
//                //read the rest of the lines except the last one
//                if (EndLine - BeginLine > 2)
//                {//if the region has more then 2 lines
//                 //sw.WriteLine("we have more then 2 lines");			
//                    for (Int32 i = BeginLine + 1; i <= EndLine - 1; i++)
//                    {

//                        sb.Append(sr.ReadLine() + System.Environment.NewLine);
//                    }
//                }
//                //add the last line
//                char[] text = new char[EndColumn];
//                sr.Read(text, 0, EndColumn);
//                sb.Append(text);
//                sb.Append(System.Environment.NewLine);
//                sr.Close();
//                //sw.Close();			
//                return sb.ToString();
//            }
//        }
//        public static string[] GetNamespaces(IClass klass, String language)
//        {
//            if (language.ToUpper() == "C#") return helperClass.GetNamespacesInCSharp(klass);
//            //		if(language.ToUpper()=="VB")return helperClass.GetNamespacesInVB(klass);
//            else throw new Exception("language " + language + " is not suported");
//        }

//        private static string[] GetNamespacesInCSharp(IClass klass)
//        {
//            string[] result = new string[2];
//            result[0] = "namespace " + klass.Namespace + " { " + Environment.NewLine;
//            result[1] = "}";
//            return result;
//        }
//        private static string[] GetNamespacesInVB(IClass klass)
//        {
//            string[] result = new string[2];
//            result[0] = "Namespace " + klass.Namespace + Environment.NewLine;
//            result[1] = "End Namespace";
//            return result;
//        }
//        public static string GenerateUsings(MonoDevelop.Projects.Parser.IUsingCollection usings, string language)
//        {
//            if (language.ToUpper() == "C#") return GenerateCSharpUsings(usings);
//            //	if(language.ToUpper()=="VB") return GenerateVbUsings(usings);
//            else throw new Exception("language " + language + " is not suported");
//        }
//        private static string GenerateCSharpUsings(MonoDevelop.Projects.Parser.IUsingCollection usings)
//        {
//            if (usings == null)
//            {
//                throw new ArgumentNullException("the IUsingCollection is null");
//            }
//            if (usings.Count == 0)
//            {
//                return string.Empty;
//            }
//            System.Text.StringBuilder sb = new System.Text.StringBuilder();
//            for (int i = 0; i < usings.Count; i++)
//            {
//                System.Collections.Generic.IEnumerator<string> ie = usings[i].Usings.GetEnumerator();

//                while (ie.MoveNext())
//                {
//                    Console.WriteLine(ie.Current);
//                    sb.Append("using " + ie.Current + ";" + System.Environment.NewLine);
//                }
//            }
//            return sb.ToString();
//        }

//        private static string GenerateVbUsings(MonoDevelop.Projects.Parser.IUsingCollection usings)
//        {
//            if (usings == null)
//            {
//                throw new ArgumentNullException("the IUsingCollection is null");
//            }
//            if (usings.Count == 0)
//            {
//                return string.Empty;
//            }
//            System.Text.StringBuilder sb = new System.Text.StringBuilder();
//            for (int i = 0; i < usings.Count; i++)
//            {
//                System.Collections.Generic.IEnumerator<string> ie = usings[i].Usings.GetEnumerator();

//                while (ie.MoveNext())
//                {
//                    Console.WriteLine(ie.Current);
//                    sb.Append("import " + ie.Current + System.Environment.NewLine);
//                }
//            }
//            return sb.ToString();
//        }
//        public static string GetHeareOfFile(string language)
//        {
//            if (language.ToUpper() == "C#")
//                return "//this file was generated by MonoDevelop in function MoveFunctionToSeparateFile";
//            else throw new Exception("Language " + language.ToUpper() + " is not suported");
//        }
//        public static void Test()
//        {
//            String FileName = "/home/simi/hf.txt";
//            //System.Text.StringBuilder sb=new System.Text.StringBuilder();
//            String classDesc = null;
//            int bl, bc, el, ec;
//            bl = bc = 1;
//            el = 10;
//            ec = 10;
//            try
//            {
//                classDesc = helperClass.GetTextFromRegion(bl, bc, el, ec, FileName);
//            }
//            catch (Exception x)
//            {
//                Console.WriteLine("eroare functia geteext" + x.Message);
//            }
//            if (classDesc == null)
//            {
//                Console.WriteLine("Sb is null");
//                return;
//            }
//            Console.Write(classDesc);
//        }
//    }

//    public enum RefactoryCommands
//    {
//        CurrentRefactoryOperations
//    }

//    public class CurrentRefactoryOperationsHandler : CommandHandler
//    {
//        protected override void Run(object data)
//        {
//            RefactoryOperation del = (RefactoryOperation)data;
//            if (del != null)
//                del();
//        }

//        protected override void Update(CommandArrayInfo ainfo)
//        {
//            Document doc = IdeApp.Workbench.ActiveDocument;
//            if (doc != null && doc.FileName != null)
//            {
//                ITextBuffer editor = IdeApp.Workbench.ActiveDocument.GetContent<ITextBuffer>();
//                if (editor != null)
//                {
//                    bool added = false;
//                    int line, column;

//                    editor.GetLineColumnFromPosition(editor.CursorPosition, out line, out column);
//                    IParseInformation pinfo;
//                    IParserContext ctx;
//                    if (doc.Project != null)
//                    {
//                        ctx = IdeApp.ProjectOperations.ParserDatabase.GetProjectParserContext(doc.Project);
//                        pinfo = IdeApp.ProjectOperations.ParserDatabase.UpdateFile(doc.Project, doc.FileName, editor.Text);
//                    }
//                    else
//                    {
//                        ctx = IdeApp.ProjectOperations.ParserDatabase.GetFileParserContext(doc.FileName);
//                        pinfo = IdeApp.ProjectOperations.ParserDatabase.UpdateFile(doc.FileName, editor.Text);
//                    }

//                    // Look for an identifier at the cursor position

//                    string id = editor.SelectedText;
//                    if (id.Length == 0)
//                    {
//                        IExpressionFinder finder = Services.ParserService.GetExpressionFinder(editor.Name);
//                        if (finder == null)
//                            return;
//                        id = finder.FindFullExpression(editor.Text, editor.CursorPosition).Expression;
//                        if (id == null) return;
//                    }

//                    ILanguageItem item = ctx.ResolveIdentifier(id, line, column, editor.Name, null);
//                    ILanguageItem eitem = ctx.GetEnclosingLanguageItem(line, column, editor);

//                    if (item != null && eitem != null && eitem.Name == item.Name && !(eitem is IProperty) && !(eitem is IMethod))
//                    {
//                        // If this occurs, then @item is the base-class version of @eitem
//                        // in which case we don't want to show the base-class @item, we'd
//                        // rather show the item the user /actually/ requested, @eitem.
//                        item = eitem;
//                        eitem = null;
//                    }

//                    IClass eclass = null;

//                    if (item is IClass)
//                    {
//                        if (((IClass)item).ClassType == ClassType.Interface)
//                            eclass = FindEnclosingClass(ctx, editor.Name, line, column);
//                        else
//                            eclass = (IClass)item;
//                    }

//                    while (item != null)
//                    {
//                        CommandInfo ci;

//                        // Add the selected item
//                        if ((ci = BuildRefactoryMenuForItem(ctx, pinfo, eclass, item)) != null)
//                        {
//                            ainfo.Add(ci, null);
//                            added = true;
//                        }

//                        if (item is IParameter)
//                        {
//                            // Add the encompasing method for the previous item in the menu
//                            item = ((IParameter)item).DeclaringMember;
//                            if (item != null && (ci = BuildRefactoryMenuForItem(ctx, pinfo, null, item)) != null)
//                            {
//                                ainfo.Add(ci, null);
//                                added = true;
//                            }
//                        }

//                        if (item is IMember && !(eitem != null && eitem is IMember))
//                        {
//                            // Add the encompasing class for the previous item in the menu
//                            item = ((IMember)item).DeclaringType;
//                            if (item != null && (ci = BuildRefactoryMenuForItem(ctx, pinfo, null, item)) != null)
//                            {
//                                ainfo.Add(ci, null);
//                                added = true;
//                            }
//                        }

//                        item = eitem;
//                        eitem = null;
//                        eclass = null;
//                    }

//                    if (added)
//                        ainfo.AddSeparator();
//                }
//            }
//        }

//        // public class Funkadelic : IAwesomeSauce, IRockOn { ...
//        //        ----------------   -------------
//        // finds this ^ if you clicked on this ^
//        IClass FindEnclosingClass(IParserContext ctx, string fileName, int line, int col)
//        {
//            IClass[] classes = ctx.GetFileContents(fileName);
//            IClass klass = null;

//            if (classes == null)
//                return null;

//            for (int i = 0; i < classes.Length; i++)
//            {
//                if ((line > classes[i].Region.BeginLine ||
//                     (line == classes[i].Region.BeginLine && col >= classes[i].Region.BeginColumn)) &&
//                    (line < classes[i].Region.EndLine ||
//                   (line == classes[i].Region.EndLine && col <= classes[i].Region.EndColumn)))
//                {
//                    klass = classes[i];
//                    break;
//                }
//            }

//            if (klass != null && klass.ClassType != ClassType.Interface)
//                return klass;

//            return null;
//        }

//        string EscapeName(string name)
//        {
//            if (name.IndexOf('_') == -1)
//                return name;

//            StringBuilder sb = new StringBuilder();
//            for (int i = 0; i < name.Length; i++)
//            {
//                if (name[i] == '_')
//                    sb.Append('_');
//                sb.Append(name[i]);
//            }

//            return sb.ToString();
//        }

//        CommandInfo BuildRefactoryMenuForItem(IParserContext ctx, IParseInformation pinfo, IClass eclass, ILanguageItem item)
//        {
//            Refactorer refactorer = new Refactorer(ctx, pinfo, eclass, item, null);
//            CommandInfoSet ciset = new CommandInfoSet();
//            Ambience ambience = null;
//            Project project = IdeApp.Workbench.ActiveDocument.Project;
//            if (project != null)
//            {
//                ambience = project.Ambience;
//            }
//            else
//                ambience = new NetAmbience();
//            string itemName = EscapeName(ambience.Convert(item, ConversionFlags.ShowGenericParameters | ConversionFlags.IncludeHTMLMarkup));
//            bool canRename = false;
//            string txt;
//            if (IdeApp.ProjectOperations.CanJumpToDeclaration(item))
//                ciset.CommandInfos.Add(GettextCatalog.GetString("_Go to declaration"), new RefactoryOperation(refactorer.GoToDeclaration));

//            if ((item is IMember) && !(item is IClass))
//                ciset.CommandInfos.Add(GettextCatalog.GetString("_Find references"), new RefactoryOperation(refactorer.FindReferences));

//            // We can rename local variables (always), method params (always), 
//            // or class/members (if they belong to a project)
//            if ((item is LocalVariable) || (item is IParameter))
//                canRename = true;
//            else if (item is IClass)
//                canRename = ((IClass)item).SourceProject != null;
//            else if (item is IMember)
//            {
//                IClass cls = ((IMember)item).DeclaringType;
//                canRename = cls != null && cls.SourceProject != null;
//            }

//            if (canRename && !(item is IClass))
//            {
//                // Defer adding this item for Classes until later
//                ciset.CommandInfos.Add(GettextCatalog.GetString("_Rename"), new RefactoryOperation(refactorer.Rename));
//            }

//            if (item is IClass)
//            {
//                IClass cls = (IClass)item;

//                if (cls.ClassType == ClassType.Enum)
//                    txt = GettextCatalog.GetString("Enum <b>{0}</b>", itemName);
//                else if (cls.ClassType == ClassType.Struct)
//                    txt = GettextCatalog.GetString("Struct <b>{0}</b>", itemName);
//                else if (cls.ClassType == ClassType.Interface)
//                    txt = GettextCatalog.GetString("Interface <b>{0}</b>", itemName);
//                else
//                    txt = GettextCatalog.GetString("Class <b>{0}</b>", itemName);

//                if (cls.BaseTypes.Count > 0 && cls.ClassType == ClassType.Class)
//                {
//                    foreach (IReturnType rt in cls.BaseTypes)
//                    {
//                        IClass bc = ctx.GetClass(rt.FullyQualifiedName, null, true, true);
//                        if (bc != null && bc.ClassType != ClassType.Interface && IdeApp.ProjectOperations.CanJumpToDeclaration(bc))
//                        {
//                            ciset.CommandInfos.Add(GettextCatalog.GetString("Go to _base"), new RefactoryOperation(refactorer.GoToBase));
//                            break;
//                        }
//                    }
//                }


//                if ((cls.ClassType == ClassType.Class && !cls.IsSealed) || cls.ClassType == ClassType.Interface)
//                    ciset.CommandInfos.Add(GettextCatalog.GetString("Find _derived classes"), new RefactoryOperation(refactorer.FindDerivedClasses));

//                ciset.CommandInfos.Add(GettextCatalog.GetString("_Find references"), new RefactoryOperation(refactorer.FindReferences));

//                if (canRename)
//                    ciset.CommandInfos.Add(GettextCatalog.GetString("_Rename"), new RefactoryOperation(refactorer.Rename));


//                //simion314

//                ciset.CommandInfos.Add("Move to separate file", new RefactoryOperation(refactorer.MoveClassToFile));


//                if (canRename && cls.ClassType == ClassType.Interface && eclass != null)
//                {
//                    // An interface is selected, so just need to provide these 2 submenu items
//                    ciset.CommandInfos.Add(GettextCatalog.GetString("Implement Interface (implicit)"), new RefactoryOperation(refactorer.ImplementImplicitInterface));
//                    ciset.CommandInfos.Add(GettextCatalog.GetString("Implement Interface (explicit)"), new RefactoryOperation(refactorer.ImplementExplicitInterface));
//                }
//                else if (canRename && cls.BaseTypes.Count > 0 && cls.ClassType != ClassType.Interface && cls == eclass)
//                {




//                    // Class might have interfaces... offer to implement them
//                    CommandInfoSet impset = new CommandInfoSet();
//                    CommandInfoSet expset = new CommandInfoSet();
//                    bool added = false;

//                    foreach (IReturnType rt in cls.BaseTypes)
//                    {
//                        IClass iface = ctx.GetClass(rt.FullyQualifiedName, rt.GenericArguments, true, true);
//                        if (iface != null && iface.ClassType == ClassType.Interface)
//                        {
//                            Refactorer ifaceRefactorer = new Refactorer(ctx, pinfo, cls, iface, rt);
//                            impset.CommandInfos.Add(ambience.Convert(rt, ConversionFlags.ShowGenericParameters), new RefactoryOperation(ifaceRefactorer.ImplementImplicitInterface));
//                            expset.CommandInfos.Add(ambience.Convert(rt, ConversionFlags.ShowGenericParameters), new RefactoryOperation(ifaceRefactorer.ImplementExplicitInterface));
//                            added = true;
//                        }
//                    }

//                    if (added)
//                    {
//                        impset.Text = GettextCatalog.GetString("Implement Interface (implicit)");
//                        ciset.CommandInfos.Add(impset, null);

//                        expset.Text = GettextCatalog.GetString("Implement Interface (explicit)");
//                        ciset.CommandInfos.Add(expset, null);
//                    }
//                }
//            }
//            else if (item is IField)
//            {
//                txt = GettextCatalog.GetString("Field <b>{0}</b>", itemName);
//                if (canRename)
//                    ciset.CommandInfos.Add(GettextCatalog.GetString("Encapsulate Field"), new RefactoryOperation(refactorer.EncapsulateField));
//                AddRefactoryMenuForClass(ctx, pinfo, ciset, ((IField)item).ReturnType.FullyQualifiedName);
//            }
//            else if (item is IProperty)
//            {
//                txt = GettextCatalog.GetString("Property <b>{0}</b>", itemName);
//                AddRefactoryMenuForClass(ctx, pinfo, ciset, ((IProperty)item).ReturnType.FullyQualifiedName);
//            }
//            else if (item is IEvent)
//            {
//                txt = GettextCatalog.GetString("Event <b>{0}</b>", itemName);
//            }
//            else if (item is IMethod)
//            {
//                IMethod method = item as IMethod;

//                if (method.IsConstructor)
//                {
//                    txt = GettextCatalog.GetString("Constructor <b>{0}</b>", EscapeName(method.DeclaringType.Name));
//                }
//                else
//                {
//                    txt = GettextCatalog.GetString("Method <b>{0}</b>", itemName);
//                    if (method.IsOverride)
//                        ciset.CommandInfos.Add(GettextCatalog.GetString("Go to _base"), new RefactoryOperation(refactorer.GoToBase));
//                }
//            }
//            else if (item is IIndexer)
//            {
//                txt = GettextCatalog.GetString("Indexer <b>{0}</b>", itemName);
//            }
//            else if (item is IParameter)
//            {
//                txt = GettextCatalog.GetString("Parameter <b>{0}</b>", itemName);
//                AddRefactoryMenuForClass(ctx, pinfo, ciset, ((IParameter)item).ReturnType.FullyQualifiedName);
//            }
//            else if (item is LocalVariable)
//            {
//                LocalVariable var = (LocalVariable)item;
//                AddRefactoryMenuForClass(ctx, pinfo, ciset, var.ReturnType.FullyQualifiedName);
//                txt = GettextCatalog.GetString("Variable <b>{0}</b>", itemName);
//            }
//            else
//            {
//                return null;
//            }

//            ciset.Text = txt;
//            ciset.UseMarkup = true;
//            return ciset;
//        }

//        void AddRefactoryMenuForClass(IParserContext ctx, IParseInformation pinfo, CommandInfoSet ciset, string className)
//        {
//            IClass cls = ctx.GetClass(className, true, true);
//            if (cls != null)
//            {
//                CommandInfo ci = BuildRefactoryMenuForItem(ctx, pinfo, null, cls);
//                if (ci != null)
//                    ciset.CommandInfos.Add(ci, null);
//            }
//        }

//        delegate void RefactoryOperation();
//    }

//    class Refactorer
//    {
//        MemberReferenceCollection references;
//        ISearchProgressMonitor monitor;
//        IParseInformation pinfo;
//        IParserContext ctx;
//        ILanguageItem item;
//        IClass klass;
//        IReturnType hintReturnType;

//        public Refactorer(IParserContext ctx, IParseInformation pinfo, IClass klass, ILanguageItem item, IReturnType hintReturnType)
//        {
//            this.pinfo = pinfo;
//            this.klass = klass;
//            this.item = item;
//            this.ctx = ctx;
//            this.hintReturnType = hintReturnType;
//        }
//        //simion314
//        public void MoveClassToFile()
//        {
//            System.IO.StreamWriter sw = null;
//            Project project = IdeApp.Workbench.ActiveDocument.Project;
//            String directory = System.IO.Path.GetDirectoryName(klass.Region.FileName);
//            string newFname = helperClass.DetermineValidFileName(klass.Region.FileName, klass.Name);
//            string oldFile = klass.Region.FileName;
//            string backup = helperClass.DetermineValidFileName(oldFile, Path.GetFileName(oldFile + ".bakup"));
//            project.ProjectFiles.Remove(oldFile);

//            //here the new class data will be stored including import statements and namespace
//            System.Text.StringBuilder sb = new System.Text.StringBuilder(1000); ;

//            //get the usings from the curent file	
//            ICompilationUnit unit = (ICompilationUnit)pinfo.MostRecentCompilationUnit;
//            MonoDevelop.Projects.Parser.IUsingCollection usings = unit.Usings;

//            //in this string the usings statement will be placed			
//            String usingsStr;

//            //here will be placed the egin and end of the curent namespace
//            String[] theNamespace;

//            //get the language of the project
//            //i observed that the array contains an empty string at index 0 and the language
//            //is specified at index 1
//            string language = project.SupportedLanguages[1];
//            //add a header to the generated file

//            sb.Append(helperClass.GetHeareOfFile(language) + Environment.NewLine);
//            sb.Append(Environment.NewLine);
//            if (usings != null && usings.Count > 0)
//            {
//                //create the strings that wil contain the imports 					
//                try
//                {
//                    usingsStr = helperClass.GenerateUsings(usings, language);
//                }
//                catch (Exception x)
//                {
//                    throw new Exception("error in generating usings method " + x.Message);
//                }
//                sb.Append(usingsStr);
//                sb.Append(Environment.NewLine);
//            }
//            else
//            {
//                //TODO the usings are null or 0			
//            }
//            string classDescription = null;
//            string tempfile = klass.Region.FileName + ".tmp";

//            //save the file before reading from it				
//            IdeApp.Workbench.ActiveDocument.Save();
//            try
//            {
//                classDescription = helperClass.ExtractTextFromRegion(klass.Region, tempfile);
//            }
//            catch (Exception x)
//            {
//                throw new Exception("error in ExtractingTextFromRegion " + x.Message);
//            }
//            if (classDescription != null)
//            {
//                theNamespace = helperClass.GetNamespaces(klass, language);
//                sb.Append(theNamespace[0]);
//                sb.Append(Environment.NewLine);
//                sb.Append(classDescription);
//                sb.Append(theNamespace[1]);
//            }
//            else
//            {
//                throw new Exception("a problem has ocured");
//            }

//            //writing all to the file
//            try
//            {
//                sw = new System.IO.StreamWriter(newFname);
//            }
//            catch (Exception x)
//            {
//                throw new Exception("error when tring to create a new file " + x.Message);
//            }
//            sw.Write(sb.ToString());
//            sw.Close();

//            //project.FileRemovedFromProject(this,null);
//            if (project != null)
//            {//if the file belongs to a project we will remove it from the project			
//                try
//                {
//                    IdeApp.Workbench.ActiveDocument.Close();
//                    project.ProjectFiles.Remove(oldFile);
//                    IdeApp.ProjectOperations.SaveProject(project);

//                }
//                catch (Exception x)
//                {
//                    Console.WriteLine("error removeing file from project " + x.Message);
//                }
//                Console.WriteLine("file " + oldFile + " removed from project");
//            }
//            else
//            {//the curent file is not belonging to a project
//                IdeApp.Workbench.ActiveDocument.Close();
//            }
//            //backup the file
//            try
//            {
//                File.Move(oldFile, backup);
//            }
//            catch (Exception x)
//            {
//                if (project != null)
//                {
//                    project.AddFile(oldFile, BuildAction.Nothing);
//                }
//                throw new IOException("can't move file " + oldFile + " to " + backup + "  " + x.Message);
//            }
//            //moving the temoray file /renaming it/ into the original file
//            try
//            {
//                File.Move(tempfile, oldFile);
//            }
//            catch
//            {
//                File.Move(backup, oldFile);
//                if (project != null)
//                {
//                    project.AddFile(oldFile, BuildAction.Nothing);
//                    IdeApp.ProjectOperations.SaveProject(project);
//                }
//            }
//            //adding files to project
//            if (project != null)
//            {
//                try
//                {

//                    project.AddFile(newFname, BuildAction.Compile);
//                    project.AddFile(oldFile, BuildAction.Compile);
//                    IdeApp.ProjectOperations.SaveProject(project);
//                }
//                catch (Exception x)
//                {
//                    throw new Exception("error adding the new file and readding the old file back to project " + x.Message);
//                }
//            }
//            //open the files in the workspace
//            IdeApp.Workbench.OpenDocument(oldFile);
//            IdeApp.Workbench.OpenDocument(newFname);
//        }


//        public void GoToDeclaration()
//        {
//            IdeApp.ProjectOperations.JumpToDeclaration(item);
//        }

//        public void FindReferences()
//        {
//            monitor = IdeApp.Workbench.ProgressMonitors.GetSearchProgressMonitor(true);
//            Thread t = new Thread(new ThreadStart(FindReferencesThread));
//            t.IsBackground = true;
//            t.Start();
//        }

//        void FindReferencesThread()
//        {
//            using (monitor)
//            {
//                CodeRefactorer refactorer = IdeApp.ProjectOperations.CodeRefactorer;

//                if (item is IMember)
//                {
//                    IMember member = (IMember)item;

//                    // private is filled only in keyword case
//                    if (member.IsPrivate || (!member.IsProtectedOrInternal && !member.IsPublic))
//                    {
//                        // look in project to be partial classes safe
//                        references = refactorer.FindMemberReferences(monitor, member.DeclaringType, member,
//                                                                       RefactoryScope.Project);
//                    }
//                    else
//                    {
//                        // for all other types look in solution because
//                        // internal members can be used in friend assemblies
//                        references = refactorer.FindMemberReferences(monitor, member.DeclaringType, member,
//                                                                       RefactoryScope.Solution);
//                    }
//                }
//                else if (item is IClass)
//                {
//                    references = refactorer.FindClassReferences(monitor, (IClass)item, RefactoryScope.Solution);
//                }

//                if (references != null)
//                {
//                    foreach (MemberReference mref in references)
//                    {
//                        monitor.ReportResult(mref.FileName, mref.Line, mref.Column, mref.TextLine);
//                    }
//                }
//            }
//        }

//        public void GoToBase()
//        {
//            IClass cls = item as IClass;
//            if (cls != null && cls.BaseTypes != null)
//            {
//                foreach (IReturnType bc in cls.BaseTypes)
//                {
//                    IClass bcls = ctx.GetClass(bc.FullyQualifiedName, true, true);
//                    if (bcls != null && bcls.ClassType != ClassType.Interface && bcls.Region != null)
//                    {
//                        IdeApp.Workbench.OpenDocument(bcls.Region.FileName, bcls.Region.BeginLine, bcls.Region.BeginColumn, true);
//                        return;
//                    }
//                }
//                return;
//            }
//            IMethod method = item as IMethod;
//            if (method != null)
//            {
//                foreach (IReturnType bc in method.DeclaringType.BaseTypes)
//                {
//                    IClass bcls = ctx.GetClass(bc.FullyQualifiedName, true, true);
//                    if (bcls != null && bcls.ClassType != ClassType.Interface && bcls.Region != null)
//                    {
//                        IMethod baseMethod = null;
//                        foreach (IMethod m in bcls.Methods)
//                        {
//                            if (m.Name == method.Name && m.Parameters.Count == m.Parameters.Count)
//                            {
//                                baseMethod = m;
//                                break;
//                            }
//                        }
//                        if (baseMethod != null)
//                            IdeApp.Workbench.OpenDocument(bcls.Region.FileName, baseMethod.Region.BeginLine, baseMethod.Region.BeginColumn, true);
//                        return;
//                    }
//                }
//                return;
//            }
//        }

//        public void FindDerivedClasses()
//        {
//            monitor = IdeApp.Workbench.ProgressMonitors.GetSearchProgressMonitor(true);
//            Thread t = new Thread(new ThreadStart(FindDerivedThread));
//            t.IsBackground = true;
//            t.Start();
//        }

//        void FindDerivedThread()
//        {
//            using (monitor)
//            {
//                IClass cls = (IClass)item;
//                if (cls == null) return;

//                IClass[] classes = IdeApp.ProjectOperations.CodeRefactorer.FindDerivedClasses(cls);
//                foreach (IClass sub in classes)
//                {
//                    if (sub.Region != null)
//                        monitor.ReportResult(sub.Region.FileName, sub.Region.BeginLine, sub.Region.BeginColumn, sub.FullyQualifiedName);
//                }
//            }
//        }

//        void ImplementInterface(bool explicitly)
//        {
//            CodeRefactorer refactorer = IdeApp.ProjectOperations.CodeRefactorer;
//            IClass iface = item as IClass;

//            if (klass == null)
//                return;

//            if (iface == null)
//                return;

//            IEditableTextBuffer editor = IdeApp.Workbench.ActiveDocument.GetContent<IEditableTextBuffer>();
//            if (editor != null)
//                editor.BeginAtomicUndo();

//            try
//            {
//                refactorer.ImplementInterface(pinfo, klass, iface, explicitly, iface, this.hintReturnType);
//            }
//            finally
//            {
//                if (editor != null)
//                    editor.EndAtomicUndo();
//            }
//        }

//        public void ImplementImplicitInterface()
//        {
//            ImplementInterface(false);
//        }

//        public void ImplementExplicitInterface()
//        {
//            ImplementInterface(true);
//        }

//        public void EncapsulateField()
//        {
//            IEditableTextBuffer editor = IdeApp.Workbench.ActiveDocument.GetContent<IEditableTextBuffer>();
//            if (editor != null)
//                editor.BeginAtomicUndo();

//            try
//            {
//                EncapsulateFieldDialog dialog = new EncapsulateFieldDialog(ctx, (IField)item);
//                dialog.Show();
//            }
//            finally
//            {
//                if (editor != null)
//                    editor.EndAtomicUndo();
//            }

//        }

//        public void Rename()
//        {
//            RenameItemDialog dialog = new RenameItemDialog(ctx, item);
//            dialog.Show();
//        }
//    }
//}
