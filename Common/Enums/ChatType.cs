using System.ComponentModel.DataAnnotations;

namespace Common.Enums
{
    public enum ChatType
    {
        Text = 1,
        Sticker = 2,
        Image = 3,
        Video = 4,
        Voice = 5
    }

    public enum ChatStatus
    {
        Sent=0,
        Recived=1,
        Seen=2
    }
}