using System;
using Core;
using Trash;

namespace BioEngineerLab.Activities
{
    public class TrashLabActivity : LabActivity
    {
        public ETrashType TrashType;
        public ETrashableObject TrashObject;

        public TrashLabActivity()
            : base(EActivity.TrashActivity)
        {
        }

        public TrashLabActivity(TrashLabActivity trashLabActivity)
            : base(EActivity.TrashActivity)
        {
            TrashType = trashLabActivity.TrashType;
            TrashObject = trashLabActivity.TrashObject;
        }
        
        public TrashLabActivity(ETrashType trashType, ETrashableObject trashObject)
            : base(EActivity.TrashActivity)
        {
            TrashType = trashType;
            TrashObject = trashObject;
        }

        public override bool Equals(Object obj)
        {
            if (obj is not TrashLabActivity trashLabActivity)
            {
                return false;
            }

            return TrashType == trashLabActivity.TrashType &&
                   TrashObject == trashLabActivity.TrashObject;
        }

        public override int GetHashCode()
        {
            return (int)TrashType + (int)TrashObject;
        }
    }
}