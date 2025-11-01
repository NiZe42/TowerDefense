using UnityEngine;

public class RequireInterfaceAttribute : PropertyAttribute
{
    public System.Type RequiredType;

    public RequireInterfaceAttribute(System.Type requiredType)
    {
        RequiredType = requiredType;
    }
}