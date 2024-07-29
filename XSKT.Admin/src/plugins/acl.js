import router from "../router/index";
import { createAcl, defineAclRules } from 'vue-simple-acl/dist/vue-simple-acl.vue2';

const userLocal = localStorage.getItem('auth-user');

const user = userLocal ? JSON.parse(userLocal) : null;

const rules = () => defineAclRules((setRule) => {
    if (user != null && user.listAction != null) {
        user.listAction.forEach((permission) => {
        //    console.log("LOG : ACL ",permission  )
            setRule(permission, () => true);
        })
    }
});

const simpleAcl = createAcl({
    user,
    rules,
    router,
    onDeniedRoute: '/unauthorized'
});

export default simpleAcl;
