import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import detector from 'i18next-browser-languagedetector';

import commonEn from './texts/en/common.json';

i18n.use(detector)
    .use(initReactI18next)
    .init({
        load: 'languageOnly',
        fallbackLng: 'en',
        debug: process.env.NODE_ENV === 'development',

        interpolation: {
            escapeValue: false, // React does this automatically
        },

        resources: {
            en: {
                common: commonEn,
            },
        },

        react: {
            useSuspense: false,
        },

        defaultNS: 'common',
    });

export default i18n;
