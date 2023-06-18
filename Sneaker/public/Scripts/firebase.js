// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAnalytics } from "firebase/analytics";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
    apiKey: "AIzaSyAvdP4PWrVSQ4HQouol-3JPrpxdb3hA92c",
    authDomain: "yikes-shop.firebaseapp.com",
    projectId: "yikes-shop",
    storageBucket: "yikes-shop.appspot.com",
    messagingSenderId: "132710205935",
    appId: "1:132710205935:web:8dffed15100bce30ea4e1a",
    measurementId: "G-YGCHX23DPP"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);