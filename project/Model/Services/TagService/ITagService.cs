using System.Collections.Generic;
using Es.Udc.DotNet.ModelUtil.Transactions;
using Es.Udc.DotNet.PracticaMaD.Model.DAOs.TagDao;
using Ninject;

namespace Es.Udc.DotNet.PracticaMaD.Model.Services.TagService
{
    public interface ITagService
    {
        [Inject]
        ITagDao TagDao { set; }

        [Transactional]
        List<Tag> GetAllTags();

        [Transactional]
        Tag FindTagByName(string tagName);

        [Transactional]
        Tag CreateTag(string tagName);

    }
}