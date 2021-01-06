namespace SokoSharp.Models
{
    public class Stage
    {
        public Stage(StageResources resources)
        {
            this.Current = resources.InitialPos;
            this.level = resources.DefaultLayer;
            this.size = resources.Size;
        }

        public Vec Current { get; private set; }

        (int block, bool landing)[] level;
        Vec size;

        public const int Wall = 0;
        public const int Free = 1;
        public const int Landing = 2;
        public const int Package = 3;
        public const int Player = 4;

        public void MoveLeft() => Move((-1, 0));

        public void MoveRight() => Move((+1, 0));

        public void MoveUp() => Move((0, +1));

        public void MoveDown() => Move((0, -1));

        void Move(Vec move)
        {
            var next = Current + move;
            var offset_next = next.y * size.x + next.x;
            var o = level[offset_next];
            if (o.block == Wall) return;
            if (o.block == Package)
            {
                var next1 = next + move;
                var offset_next1 = next1.y * size.x + next1.x;
                var o1 = level[offset_next1];
                if (o1.block != Free) return; // cannot move

                level[offset_next1].block = level[offset_next].block;
            }
            var offset_current = Current.y * size.x + Current.x;
            level[offset_next].block = level[offset_current].block;
            level[offset_current].block = Free;
            Current = next;
        }

        public void Update()
        { 
        }
    }
}
