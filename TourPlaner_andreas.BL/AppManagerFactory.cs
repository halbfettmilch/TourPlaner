namespace TourPlaner_andreas.BL {
    public static class AppManagerFactory {
        private static AppManager manager;

        public static AppManager GetFactoryManager() {
            if (manager == null) {
                manager = new AppManagerFactoryImpl();
            }
            return manager;
        }
    }
}
