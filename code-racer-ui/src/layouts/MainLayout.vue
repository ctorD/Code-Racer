<template>
  <q-layout view="lHh Lpr lFf">
    <q-header elevated>
      <q-toolbar>
        <q-btn
          flat
          dense
          round
          icon="menu"
          aria-label="Menu"
          @click="toggleLeftDrawer"
        />

        <q-toolbar-title>
          Code Racer
        </q-toolbar-title>

        <div>{{ user }}</div>
      </q-toolbar>
    </q-header>

    <q-drawer
      v-model="leftDrawerOpen"
      show-if-above
      bordered
    >
      <q-list>
        <q-item-label
          header
        >
          Essential Links
        </q-item-label>

        <EssentialLink
          v-for="link in linksList"
          :key="link.title"
          v-bind="link"
        />
      </q-list>
    </q-drawer>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
  <q-dialog v-model="showLoginDialog">
    <login-dialog></login-dialog>
  </q-dialog>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import EssentialLink, { EssentialLinkProps } from 'components/EssentialLink.vue';
import {useAuth} from 'src/auth/auth';
import LoginDialog from 'components/Auth/LoginDialog.vue';

defineOptions({
  name: 'MainLayout'
});


const linksList: EssentialLinkProps[] = [
  {
    title: 'Home',
    icon: 'home',
    link: 'https://quasar.dev'
  },
  {
    title: 'Github',
    caption: 'Code Racer',
    icon: 'code',
    link: 'https://github.com/ctorD/Code-Racer'
  },
];

const leftDrawerOpen = ref(false);
const {showLoginDialog, user} = useAuth()


function toggleLeftDrawer () {
  leftDrawerOpen.value = !leftDrawerOpen.value;
}
</script>
