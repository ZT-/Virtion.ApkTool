using IKVM.Attributes;
using IKVM.Internal;
using ikvm.runtime;
using IKVM.Runtime;
using java.lang;
using java.lang.reflect;
using java.util.jar;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace IKVM.Helper
{

    public class Starter
    {
        private class Timer
        {
            private static IKVM.Helper.Starter.Timer t;

            private DateTime now = DateTime.Now;

            internal Timer()
            {
                IKVM.Helper.Starter.Timer.t = this;
            }

            ~Timer()
            {
                Console.WriteLine(DateTime.Now - this.now);
            }
        }

        private class SaveAssemblyShutdownHook : java.lang.Thread
        {
            private Class clazz;

            internal SaveAssemblyShutdownHook(Class clazz)
                : base("SaveAssemblyShutdownHook")
            {
                this.clazz = clazz;
            }

            public override void run()
            {
                System.Threading.Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;
                Console.Error.WriteLine("Saving dynamic assembly...");
                try
                {
                    IKVM.Internal.Starter.SaveDebugImage();
                    Console.Error.WriteLine("Saving done.");
                }
                catch (System.Exception ex)
                {
                    Console.Error.WriteLine(ex);
                    Console.Error.WriteLine(new StackTrace(ex, true));
                    Debug.Assert(false, ex.ToString());
                }
            }
        }

        private class WaitShutdownHook : java.lang.Thread
        {
            internal WaitShutdownHook()
                : base("WaitShutdownHook")
            {
            }

            public override void run()
            {
                Console.Error.WriteLine("IKVM runtime terminated. Waiting for Ctrl+C...");
                while (true)
                {
                    System.Threading.Thread.Sleep(-1);
                }
            }
        }

        [HideFromJava, STAThread]
        public static int StartMain(string[] args)
        {
            Tracer.EnableTraceConsoleListener();
            Tracer.EnableTraceForDebug();
            Hashtable hashtable = new Hashtable();
            string text = Environment.GetEnvironmentVariable("CLASSPATH");
            if (text == null || text == "")
            {
                text = ".";
            }
            hashtable["java.class.path"] = text;
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            bool flag4 = false;
            bool flag5 = false;
            string text2 = null;
            int num = -1;
            bool flag6 = false;
            string arg = null;
            bool flag7 = false;
            int result;
            for (int i = 0; i < args.Length; i++)
            {
                string text3 = args[i];
                if (text3[0] != '-')
                {
                    text2 = text3;
                    num = i + 1;
                    break;
                }
                if (text3 == "-help" || text3 == "-?")
                {
                    break;
                }
                if (text3 == "-Xsave")
                {
                    flag2 = true;
                    IKVM.Internal.Starter.PrepareForSaveDebugImage();
                }
                else if (text3 == "-XXsave")
                {
                    flag3 = true;
                    IKVM.Internal.Starter.PrepareForSaveDebugImage();
                }
                else if (text3 == "-Xtime")
                {
                    new IKVM.Helper.Starter.Timer();
                }
                else if (text3 == "-Xwait")
                {
                    flag4 = true;
                }
                else if (text3 == "-Xbreak")
                {
                    Debugger.Break();
                }
                else if (text3 == "-Xnoclassgc")
                {
                    IKVM.Internal.Starter.ClassUnloading = false;
                }
                else if (text3 == "-jar")
                {
                    flag = true;
                }
                else
                {
                    if (text3 == "-version")
                    {
                        Console.WriteLine(Startup.getVersionAndCopyrightInfo());
                        Console.WriteLine();
                        Console.WriteLine("CLR version: {0} ({1} bit)", Environment.Version, IntPtr.Size * 8);
                        System.Type type = System.Type.GetType("Mono.Runtime");
                        if (type != null)
                        {
                            Console.WriteLine("Mono version: {0}", type.InvokeMember("GetDisplayName", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, null, new object[0]));
                        }
                        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                        for (int j = 0; j < assemblies.Length; j++)
                        {
                            Assembly assembly = assemblies[j];
                            Console.WriteLine("{0}: {1}", assembly.GetName().Name, assembly.GetName().Version);
                        }
                        string property = java.lang.System.getProperty("openjdk.version");
                        if (property != null)
                        {
                            Console.WriteLine("OpenJDK version: {0}", property);
                        }
                        result = 0;
                        return result;
                    }
                    if (text3 == "-showversion")
                    {
                        flag5 = true;
                    }
                    else if (text3.StartsWith("-D"))
                    {
                        text3 = text3.Substring(2);
                        string[] array = text3.Split(new char[]
					{
						'='
					});
                        string value;
                        if (array.Length == 2)
                        {
                            value = array[1];
                        }
                        else if (array.Length == 1)
                        {
                            value = "";
                        }
                        else
                        {
                            value = text3.Substring(array[0].Length + 1);
                        }
                        hashtable[array[0]] = value;
                    }
                    else if (text3 == "-ea" || text3 == "-enableassertions")
                    {
                        Assertions.EnableAssertions();
                    }
                    else if (text3 == "-da" || text3 == "-disableassertions")
                    {
                        Assertions.DisableAssertions();
                    }
                    else if (text3 == "-esa" || text3 == "-enablesystemassertions")
                    {
                        Assertions.EnableSystemAssertions();
                    }
                    else if (text3 == "-dsa" || text3 == "-disablesystemassertions")
                    {
                        Assertions.DisableSystemAssertions();
                    }
                    else if (text3.StartsWith("-ea:") || text3.StartsWith("-enableassertions:"))
                    {
                        Assertions.EnableAssertions(text3.Substring(text3.IndexOf(':') + 1));
                    }
                    else if (text3.StartsWith("-da:") || text3.StartsWith("-disableassertions:"))
                    {
                        Assertions.DisableAssertions(text3.Substring(text3.IndexOf(':') + 1));
                    }
                    else if (text3 == "-cp" || text3 == "-classpath")
                    {
                        hashtable["java.class.path"] = args[++i];
                    }
                    else if (text3.StartsWith("-Xtrace:"))
                    {
                        Tracer.SetTraceLevel(text3.Substring(8));
                    }
                    else if (text3.StartsWith("-Xmethodtrace:"))
                    {
                        Tracer.HandleMethodTrace(text3.Substring(14));
                    }
                    else if (text3 == "-Xdebug")
                    {
                        flag6 = true;
                    }
                    else if (!(text3 == "-Xnoagent"))
                    {
                        if (text3.StartsWith("-Xrunjdwp") || text3.StartsWith("-agentlib:jdwp"))
                        {
                            arg = text3;
                            flag6 = true;
                        }
                        else if (text3.StartsWith("-Xreference:"))
                        {
                            Startup.addBootClassPathAssemby(Assembly.LoadFrom(text3.Substring(12)));
                        }
                        else if (text3 == "-Xnoglobbing")
                        {
                            flag7 = true;
                        }
                        else
                        {
                            if (!text3.StartsWith("-Xms") && !text3.StartsWith("-Xmx") && !text3.StartsWith("-Xss") && !(text3 == "-Xmixed") && !(text3 == "-Xint") && !(text3 == "-Xnoclassgc") && !(text3 == "-Xincgc") && !(text3 == "-Xbatch") && !(text3 == "-Xfuture") && !(text3 == "-Xrs") && !(text3 == "-Xcheck:jni") && !(text3 == "-Xshare:off") && !(text3 == "-Xshare:auto") && !(text3 == "-Xshare:on"))
                            {
                                Console.Error.WriteLine("{0}: illegal argument", text3);
                                break;
                            }
                            Console.Error.WriteLine("Unsupported option ignored: {0}", text3);
                        }
                    }
                }
            }
            if (text2 == null || flag5)
            {
                Console.Error.WriteLine(Startup.getVersionAndCopyrightInfo());
                Console.Error.WriteLine();
            }
            if (text2 == null)
            {
                Console.Error.WriteLine("usage: ikvm [-options] <class> [args...]");
                Console.Error.WriteLine("          (to execute a class)");
                Console.Error.WriteLine("    or ikvm -jar [-options] <jarfile> [args...]");
                Console.Error.WriteLine("          (to execute a jar file)");
                Console.Error.WriteLine();
                Console.Error.WriteLine("where options include:");
                Console.Error.WriteLine("    -? -help          Display this message");
                Console.Error.WriteLine("    -version          Display IKVM and runtime version");
                Console.Error.WriteLine("    -showversion      Display version and continue running");
                Console.Error.WriteLine("    -cp -classpath <directories and zip/jar files separated by {0}>", Path.PathSeparator);
                Console.Error.WriteLine("                      Set search path for application classes and resources");
                Console.Error.WriteLine("    -D<name>=<value>  Set a system property");
                Console.Error.WriteLine("    -ea[:<packagename>...|:<classname>]");
                Console.Error.WriteLine("    -enableassertions[:<packagename>...|:<classname>]");
                Console.Error.WriteLine("                      Enable assertions.");
                Console.Error.WriteLine("    -da[:<packagename>...|:<classname>]");
                Console.Error.WriteLine("    -disableassertions[:<packagename>...|:<classname>]");
                Console.Error.WriteLine("                      Disable assertions");
                Console.Error.WriteLine("    -Xsave            Save the generated assembly (for debugging)");
                Console.Error.WriteLine("    -Xtime            Time the execution");
                Console.Error.WriteLine("    -Xtrace:<string>  Displays all tracepoints with the given name");
                Console.Error.WriteLine("    -Xmethodtrace:<string>");
                Console.Error.WriteLine("                      Builds method trace into the specified output methods");
                Console.Error.WriteLine("    -Xwait            Keep process hanging around after exit");
                Console.Error.WriteLine("    -Xbreak           Trigger a user defined breakpoint at startup");
                Console.Error.WriteLine("    -Xnoclassgc       Disable class garbage collection");
                Console.Error.WriteLine("    -Xnoglobbing      Disable argument globbing");
                result = 1;
                return result;
            }
            try
            {
                if (flag6)
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    string text4 = arg + " -pid:" + System.Diagnostics.Process.GetCurrentProcess().Id;
                    string text5 = new FileInfo(assembly.Location).DirectoryName + "\\debugger.exe";
                    try
                    {
                        ProcessStartInfo processStartInfo = new ProcessStartInfo(text5, text4);
                        processStartInfo.UseShellExecute = false;
                        new System.Diagnostics.Process
                        {
                            StartInfo = processStartInfo
                        }.Start();
                    }
                    catch (System.Exception ex)
                    {
                        Console.Error.WriteLine(text5 + " " + text4);
                        throw ex;
                    }
                }
                if (flag)
                {
                    hashtable["java.class.path"] = text2;
                }
                hashtable["sun.java.command"] = string.Join(" ", args, num - 1, args.Length - (num - 1));
                hashtable["sun.java.launcher"] = "SUN_STANDARD";
                Startup.setProperties(hashtable);
                Startup.enterMainThread();
                string[] array2;
                if (flag7)
                {
                    array2 = new string[args.Length - num];
                    System.Array.Copy(args, num, array2, 0, array2.Length);
                }
                else
                {
                    array2 = Startup.glob(args, num);
                }
                if (flag)
                {
                    text2 = IKVM.Helper.Starter.GetMainClassFromJarManifest(text2);
                    if (text2 == null)
                    {
                        result = 1;
                        return result;
                    }
                }
                Class clazz = Class.forName(text2, true, ClassLoader.getSystemClassLoader());
                try
                {
                    Method method = IKVM.Internal.Starter.FindMainMethod(clazz);
                    if (method == null)
                    {
                        throw new NoSuchMethodError("main");
                    }
                    if (!Modifier.isPublic(method.getModifiers()))
                    {
                        Console.Error.WriteLine("Main method not public.");
                    }
                    else
                    {
                        method.setAccessible(true);
                        if (flag2)
                        {
                            java.lang.Runtime.getRuntime().addShutdownHook(new IKVM.Helper.Starter.SaveAssemblyShutdownHook(clazz));
                        }
                        if (flag4)
                        {
                            java.lang.Runtime.getRuntime().addShutdownHook(new IKVM.Helper.Starter.WaitShutdownHook());
                        }
                        try
                        {
                            method.invoke(null, new object[]
						{
							array2
						});
                            result = 0;
                            return result;
                        }
                        catch (InvocationTargetException ex2)
                        {
                            throw ex2.getCause();
                        }
                    }
                }
                finally
                {
                    if (flag3)
                    {
                        IKVM.Internal.Starter.SaveDebugImage();
                    }
                }
            }
            catch (System.Exception t)
            {
                java.lang.Thread thread = java.lang.Thread.currentThread();
                thread.getThreadGroup().uncaughtException(thread, Util.mapException(t));
            }
            finally
            {
                Startup.exitMainThread();
            }
            result = 1;
            return result;
        }

        private static string GetMainClassFromJarManifest(string mainClass)
        {
            JarFile jarFile = new JarFile(mainClass);
            string result;
            try
            {
                Manifest manifest = jarFile.getManifest();
                if (manifest == null)
                {
                    Console.Error.WriteLine("Jar file doesn't contain manifest");
                    result = null;
                    return result;
                }
                mainClass = manifest.getMainAttributes().getValue(java.util.jar.Attributes.Name.MAIN_CLASS);
            }
            finally
            {
                jarFile.close();
            }
            if (mainClass == null)
            {
                Console.Error.WriteLine("Manifest doesn't contain a Main-Class.");
                result = null;
            }
            else
            {
                result = mainClass.Replace('/', '.');
            }
            return result;
        }
    }

}
