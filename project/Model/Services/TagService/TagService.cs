using System.Collections.Generic;
using Es.Udc.DotNet.ModelUtil.Exceptions;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.TagDao;
using Ninject;

namespace Es.Udc.DotNet.PracticaMaD.Model.Services.TagService
{
    public class TagService : ITagService
    {
        [Inject]
        public ITagDao TagDao { private get; set; }

        [Transactional]
        public List<Tag> GetAllTags()
        {
            List<Tag> tags = TagDao.GetAllElements();

            return tags;
        }

        [Transactional]
        public Tag FindTagByName(string tagName)
        {
            Tag tag = TagDao.FindByTagName(tagName);
            return tag;
        }

        [Transactional]
        public Tag CreateTag(string tagName)
        {
            if (TagDao.existsByTagName(tagName))
            {
                throw new DuplicateInstanceException(tagName,
                    typeof(Tag).Name);
            }
            Tag tag = new Tag();
            tag.tagName = tagName;
            TagDao.Create(tag);
            return tag;
        }

    }
}