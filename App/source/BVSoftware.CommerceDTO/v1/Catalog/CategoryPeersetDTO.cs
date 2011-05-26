using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BVSoftware.CommerceDTO.v1.Catalog
{
    [DataContract]
    public class CategoryPeerSetDTO
    {

        private List<CategorySnapshotDTO> _Parents = new List<CategorySnapshotDTO>();
        private List<CategorySnapshotDTO> _Peers = new List<CategorySnapshotDTO>();
        private List<CategorySnapshotDTO> _Children = new List<CategorySnapshotDTO>();

        [DataMember]
        public List<CategorySnapshotDTO> Parents
        {
            get { return _Parents; }
            set { _Parents = value; }
        }
        [DataMember]
        public List<CategorySnapshotDTO> Peers
        {
            get { return _Peers; }
            set { _Peers = value; }
        }
        [DataMember]
        public List<CategorySnapshotDTO> Children
        {
            get { return _Children; }
            set { _Children = value; }
        }

        public CategoryPeerSetDTO()
        {

        }

    }
}
