<template>
  <q-page class="row items-start justify-start">
    <lobby-list></lobby-list>
  </q-page>
</template>

<script setup lang="ts">

import LobbyList from 'components/Index/LobbyList.vue';

defineOptions({
  name: 'IndexPage'
});

</script>
