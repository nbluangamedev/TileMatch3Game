using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public static class AudioNameCreator
{
    private const string MENUITEM_PATH = "NBLuanGD/Create Audio Name Script";
    private const string EXPORT_PATH = "Assets/Scripts/Constants/AUDIO.cs";

    private static readonly string FILENAME = Path.GetFileName(EXPORT_PATH);
    private static readonly string FILENAME_WITHOUT_EXTENSION = Path.GetFileNameWithoutExtension(EXPORT_PATH);

    [MenuItem(MENUITEM_PATH)]
    public static void Create()
    {
        if (!CanCreate())
        {
            return;
        }

        CreateScript();

        EditorUtility.DisplayDialog(FILENAME, "Create completed!!!", "OK");
    }

    public static void CreateScript()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendFormat("public static class {0}", FILENAME_WITHOUT_EXTENSION).AppendLine();
        builder.AppendLine("{");

        object[] bgmList = Resources.LoadAll("Audio/BGM");
        object[] seList = Resources.LoadAll("Audio/SE");

        object[] enemyHitList = Resources.LoadAll("Audio/Enemy/TakeDamage");        //add
        object[] playerHitList = Resources.LoadAll("Audio/Player/TakeDamage");
        object[] playerTalkList = Resources.LoadAll("Audio/Player/Talk");

        foreach (AudioClip bgm in bgmList)
        {
            builder.Append("\t").AppendFormat(@"  public const string BGM_{0} = ""{1}"";", bgm.name.ToUpper(), bgm.name).AppendLine();
        }

        builder.AppendLine("\t");

        foreach (AudioClip se in seList)
        {
            builder.Append("\t").AppendFormat(@"  public const string SE_{0} = ""{1}"";", se.name.ToUpper(), se.name).AppendLine();
        }

        builder.AppendLine("\t");

        foreach (AudioClip etd in enemyHitList)     //add
        {
            builder.Append("\t").AppendFormat(@"  public const string ETD_{0} = ""{1}"";", etd.name.ToUpper(), etd.name).AppendLine();
        }

        builder.AppendLine("\t");

        foreach (AudioClip etd in playerHitList)     //add
        {
            builder.Append("\t").AppendFormat(@"  public const string PTD_{0} = ""{1}"";", etd.name.ToUpper(), etd.name).AppendLine();
        }

        builder.AppendLine("\t");

        foreach (AudioClip etd in playerTalkList)     //add
        {
            builder.Append("\t").AppendFormat(@"  public const string PT_{0} = ""{1}"";", etd.name.ToUpper(), etd.name).AppendLine();
        }

        builder.AppendLine("}");

        string directoryName = Path.GetDirectoryName(EXPORT_PATH);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }

        File.WriteAllText(EXPORT_PATH, builder.ToString(), Encoding.UTF8);
        AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
    }

    [MenuItem(MENUITEM_PATH, true)]
    private static bool CanCreate()
    {
        return !EditorApplication.isPlaying && !Application.isPlaying && !EditorApplication.isCompiling;
    }
}