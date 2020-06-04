namespace du {
    interface IOperation {
        void calculate(string home);
    }

    class SequentialOperation : IOperation {
        public int NumDirs {get; private set;}  // number of folders counted
        public int NumFiles {get; private set;} // number of files counted
        public int NumBytes {get; private set;} // total size of everything, in bytes

        /**
        */
        public void calculate(string home) {

        }
    }
}