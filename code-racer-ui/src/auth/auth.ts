import axios from 'axios';
import {ref} from 'vue';

type AuthResponse = {
  user: string | undefined;
}

const user = ref('');
const showLoginDialog = ref(false);
axios.interceptors.response.use((response) => {
  return response;
}, (error) => {
  if (error.response.status === 401) {
    //Unauthorised Show login
    showLoginDialog.value = true;
  }
  return Promise.reject(error)
})
export function useAuth() {
  const requestUser = new Promise<string>((resolve, reject) => {
    axios.get<AuthResponse | undefined>('/api/user').then((res) => {
      console.log(res);
      if (res.status === 401) {
        //Alert not authorised
        reject(res.statusText);
        return;
      }
      if (res.data == undefined) {
        return
      }
      if (res.data.user === undefined) {
        // No user data found in cookie
        return;
      }
      user.value = res.data.user;
      if (user.value) {
        resolve(res.data.user);
      }
      reject(res.data.user);
    }).catch(e => reject(e));
  });
  const requestLogin = (username: string | undefined) => {
    return axios.post<AuthResponse>('/api/user', null, { params: { name: username}}).then((res) => {
      if (res.data == undefined) {
        return
      }
      if (res.data.user === undefined) {
        // No user data found in cookie
        return;
      }
      user.value = res.data.user;
    })
  }

  return {requestLogin, requestUser, user, showLoginDialog}
}
