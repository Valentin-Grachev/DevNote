
namespace DevNote
{
    public interface ISaveData<T>
    {
        public T ParseSaveData(string saveData);

        public string ToSaveData();

    }
}
