<script setup lang="ts">
import {ref} from 'vue';
import {useAuth} from 'src/auth/auth';

const username = ref('');
const { requestLogin, showLoginDialog } = useAuth()
function submit(){
  requestLogin(username.value).then(() => {
    showLoginDialog.value = false;
  })
}

function skip(){
  requestLogin(undefined).then(() => {
    showLoginDialog.value = false;
  })
}

</script>

<template>
  <q-card>
    <q-input :autofocus="true" v-model="username"></q-input>
    <q-btn @click="submit()">Submit</q-btn>
    <q-btn @click="skip()">Skip</q-btn>
  </q-card>
</template>

<style scoped>


</style>
