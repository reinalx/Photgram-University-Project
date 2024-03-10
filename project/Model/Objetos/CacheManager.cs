namespace Es.Udc.DotNet.PracticaMaD.Model.Objetos
{
    public class CacheManager
    {
        public string Keyword { get; set; }
        public int StartIndex { get; set; }
        public int Size { get; set; }

        public long? CatId { get; set; }

        public CacheManager(string keyword, long? catId, int startIndex, int size)
        {
            this.Size = size;
            this.StartIndex = startIndex;
            this.Keyword = keyword;
            this.CatId = catId;
        }
    }
}