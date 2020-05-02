using SadConsole;
using System.Collections.Generic;

namespace Engine.Items
{
    internal class Inventory
    {
        internal enum ActionResult
        {
            Failure,
            Success
        }

        private readonly List<BasicEntity> carriedItems = new List<BasicEntity>();
        private BasicEntity head;
        private BasicEntity leftHand;
        private BasicEntity rightHand;
        private BasicEntity feet;
        private BasicEntity body;

        internal const int MaxCarriedItems = 11;

        internal IEnumerable<BasicEntity> CarriedItems => carriedItems;

        internal BasicEntity Head => head;
        internal BasicEntity LeftHand => leftHand;
        internal BasicEntity RightHand => rightHand;
        internal BasicEntity Feet => feet;
        internal BasicEntity Body => body;

        internal ActionResult AddItem(BasicEntity item, bool carried)
        {
            //if (carried || item.Spot == InventorySpot.None)
            //{
            //    if (carriedItems.Count == MaxCarriedItems)
            //    {
            //        return ActionResult.Failure;
            //    }

            //    carriedItems.Add(item);
            //    return ActionResult.Success;
            //}

            //switch (item.Spot)
            //{
            //    case InventorySpot.Head:
            //        head = item;
            //        break;
            //    case InventorySpot.LHand:
            //        leftHand = item;
            //        break;
            //    case InventorySpot.RHand:
            //        rightHand = item;
            //        break;
            //    case InventorySpot.Body:
            //        body = item;
            //        break;
            //    case InventorySpot.Feet:
            //        feet = item;
            //        break;
            //    default:
            //        break;
            //}
            return ActionResult.Success;
        }

        internal ActionResult RemoveItem(BasicEntity item)
        {
            if (carriedItems.Contains(item))
            {
                carriedItems.Remove(item);
                //drop item
            }
            else
            {
                //switch (item.Spot)
                //{
                //    case InventorySpot.Head:
                //        if (head == item)
                //        {
                //            head = null;
                //        }

                //        break;
                //    case InventorySpot.LHand:
                //        if (leftHand == item)
                //        {
                //            leftHand = null;
                //        }

                //        break;
                //    case InventorySpot.RHand:
                //        if (rightHand == item)
                //        {
                //            rightHand = null;
                //        }

                //        break;
                //    case InventorySpot.Body:
                //        if (body == item)
                //        {
                //            body = null;
                //        }

                //        break;
                //    case InventorySpot.Feet:
                //        if (feet == item)
                //        {
                //            feet = null;
                //        }

                //        break;
                //    default:
                //        break;
                //}
            }

            return ActionResult.Success;
        }

        internal bool IsSlotEquipped(InventorySpot spot)
        {
            switch (spot)
            {
                case InventorySpot.Head:
                    return head != null;
                case InventorySpot.LHand:
                    return leftHand != null;
                case InventorySpot.RHand:
                    return rightHand != null;
                case InventorySpot.Body:
                    return body != null;
                case InventorySpot.Feet:
                    return feet != null;
                default:
                    return false;
            }
        }

        internal bool IsInventoryFull() => carriedItems.Count == MaxCarriedItems;
        internal BasicEntity GetItem(InventorySpot spot)
        {
            switch (spot)
            {
                case InventorySpot.Head:
                    return head;
                case InventorySpot.LHand:
                    return leftHand;
                case InventorySpot.RHand:
                    return rightHand;
                case InventorySpot.Body:
                    return body;
                case InventorySpot.Feet:
                    return feet;
                default:
                    return null;
            }
        }

        internal IEnumerable<BasicEntity> GetEquippedItems()
        {
            List<BasicEntity> items = new List<BasicEntity>(5);

            if (head != null)
            {
                items.Add(head);
            }

            if (leftHand != null)
            {
                items.Add(leftHand);
            }

            if (rightHand != null)
            {
                items.Add(rightHand);
            }

            if (body != null)
            {
                items.Add(body);
            }

            if (feet != null)
            {
                items.Add(feet);
            }

            return items;
        }
    }
}
