/// <summary>
/// 配置父类，配合导表工具使用
/// </summary>
public abstract class Config
{
    public string Id;
    public abstract void ParseDataRow(string dataRowText);
}