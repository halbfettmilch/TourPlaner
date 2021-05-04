namespace TourPlaner_andreas.BL {
    public static class AppManagerFactory {
        private static IAppManager manager;

        public static IAppManager GetFactoryManager() {
            if (manager == null) {
                manager = new AppManagerFactoryImpl();
            }
            return manager;
        }
    }
}
