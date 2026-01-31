using kekchpek.SaveSystem.Codec;
using kekchpek.SaveSystem.CustomSerialization;
using kekchpek.Auxiliary.ReactiveList;

namespace kekchpek.GameSaves.Codecs
{
    public class BoolListCodec : ICustomCodec<MutableList<bool>>
    {
        public MutableList<bool> Deserialize(ILoadStream stream)
        {
            var list = new MutableList<bool>();
            var count = stream.LoadStruct<int>();
            for (int i = 0; i < count; i++)
                list.Add(stream.LoadStruct<bool>());
            return list;
        }

        public void Serialize(ISaveStream stream, object value)
        {
            var list = (MutableList<bool>)value;
            stream.SaveStruct(list?.Count ?? 0);
            if (list != null)
                foreach (var b in list)
                    stream.SaveStruct(b);
        }
    }
}

