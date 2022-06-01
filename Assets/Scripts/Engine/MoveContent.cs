using UnityEngine;

namespace Assets.Scripts.Engine
{
    internal sealed class MoveContent
    {
        public GamePieceColor PieceColor;
        public GamePieceColor PieceTakenColor;

        public byte SourceColumn;
        public byte SourceRow;
        public byte DestinationColumn;
        public byte DestinationRow;

        public MoveContent() { }

        public MoveContent(MoveContent moveContent)
        {
            PieceColor = moveContent.PieceColor;
            PieceTakenColor = moveContent.PieceTakenColor;

            SourceColumn = moveContent.SourceColumn;
            SourceRow = moveContent.SourceRow;
            DestinationColumn = moveContent.DestinationColumn;
            DestinationRow = moveContent.DestinationRow;
        }

        public MoveContent(GamePieceColor pieceColor, byte sourceColumn, byte sourceRow, byte destinationColumn, byte destinationRow)
        {
            PieceColor = pieceColor;
            PieceTakenColor = GamePieceColor.White;
            SourceColumn = sourceColumn;
            SourceRow = sourceRow;
            DestinationColumn = destinationColumn;
            DestinationRow = destinationRow;

        }

        public override string ToString()
        {
            return string.Format("{0}: {1}{2} - {3}{4}", PieceColor, Board.GetColumnFromByte(SourceColumn).ToUpper(), SourceRow + 1,
                Board.GetColumnFromByte(DestinationColumn).ToUpper(), DestinationRow + 1);
        }
    }
}
