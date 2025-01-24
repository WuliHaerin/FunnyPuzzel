#pragma strict

public class ResolutionManagerJS extends MonoBehaviour 
{
    public var toReposition				: Transform[];
    public var toolbox 					: Transform;

    public var scale3x2 				: float = 1.1f;
    public var scale4x3   				: float = 1;
    public var scale5x3   				: float = 1.25f;
    public var scale8x5   				: float = 12.5f;
    public var scale16x9  				: float = 1.4f;

    static var myInstance				: ResolutionManagerJS;

    //Retursn the instance
    public static function Instance() {  return myInstance; }

    // Use this for initialization
    function Start() 
    {
        myInstance = this;

        SetResolutionSetting();
    }
    //Returns the aspect ratio
    function GetAspectRatio(a : int, b : int)
    {
        var m : int = GetGreatestDivider(a, b);
        return (a / m) + ":" + (b / m);
    }
    //Returns the greatest divider of a and b
    function GetGreatestDivider(a : int, b : int)
    {
        var m : int;

        while (b != 0)
        {
            m = a % b;
            a = b;
            b = m;
        }

        return a;
    }
    //Moves the items
    function MoveItems(ar : String)
    {
        for (var item : Transform in toReposition)
        {
            switch (ar)
            {
                case "3:2":
                    item.position = new Vector3(item.position.x * scale3x2, item.position.y, item.position.z);
                    toolbox.position = new Vector3((scale3x2 - 1) * 4, toolbox.position.y, toolbox.position.z);
                    break;

                case "4:3":
                    item.position = new Vector3(item.position.x * scale4x3, item.position.y, item.position.z);
                    break;

                case "5:3":
                    item.position = new Vector3(item.position.x * scale5x3, item.position.y, item.position.z);
                    toolbox.position = new Vector3((scale5x3 - 1) * 4, toolbox.position.y, toolbox.position.z);
                    break;

                case "8:5":
                    item.position = new Vector3(item.position.x * scale8x5, item.position.y, item.position.z);
                    toolbox.position = new Vector3((scale8x5 - 1) * 4, toolbox.position.y, toolbox.position.z);
                    break;

                case "16:9":
                    item.position = new Vector3(item.position.x * scale16x9, item.position.y, item.position.z);
                    toolbox.position = new Vector3((scale16x9 - 1) * 4, toolbox.position.y, toolbox.position.z);
                    break;
            }
        }
    }
    //Set target resolution
    public function SetResolutionSetting()
    {
        //Calculate aspect ratio
        var ar : String = GetAspectRatio(Screen.width, Screen.height);
        MoveItems(ar);
    }
}
