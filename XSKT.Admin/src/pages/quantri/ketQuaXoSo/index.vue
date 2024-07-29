<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import { required } from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import { notifyModel } from "@/models/notifyModel";
import { ketQuaXSModel } from "@/models/ketQuaXSModel";


export default {
  page: {
    title: "Kết quả xổ số",
    meta: [{ name: "description", content: appConfig.description }],
  },
  components: { Layout, PageHeader, /* DatePicker */ },
  data() {
    return {
      apiView: `${process.env.VUE_APP_API_URL}filesminio/view/`,
      data: [],
      title: "Kết quả xổ số",
      showModal: false,
      submitted: false,
      model: ketQuaXSModel.baseJson(),
      ketquaxoso: null,
      items: [
        {
          text: "TẠO KẾT QUẢ",
          active: true,
        }
      ],
      locale: 'vi',
    };
  },
  validations: {
    model: {
      /*       date: { required },
            giai8: { required },
            giai7: { required },
            giai6: { required },
            giai5: { required },
            giai4: { required },
            giai3: { required },
            giai2: { required },
            giai1: { required },
            giaidb: { required } */
    },
  },
  created() {
    this.getKQXoSo();
    const user = {
      id: 1,
      name: 'Victory Osayi',
      is_editor: true,
      is_admin: false,
      // you can have role based permission list or access control list possibly from database
      permissions: [
        'manage-KETQUAXOSO-656004cf4235b5c9c4c589e9', 
        'create-KETQUAXOSO-656004cf4235b5c9c4c589e9',
        'update-KETQUAXOSO-656004cf4235b5c9c4c589e9',
        'delete-KETQUAXOSO-656004cf4235b5c9c4c589e9'
      ]
    }
  },
  methods: {
    clearDate() {
      this.model.date = null;
    },
    handleResetForm() {
      this.model = ketQuaXSModel.baseJson()
      this.model.date = new Date();
    },
    async handleSubmit(e) {
      console.log("Model: ", this.model);
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
          this.model._id != 0 &&
          this.model._id != null &&
          this.model._id
        ) {
          // Update model
          console.log("LOG HANLE UPDATE: ")
          await this.$store.dispatch("ketQuaStore/update", this.model).then((res) => {
            if (res.code === 0) {
              this.showModal = false;
              this.getKQXoSo();
              this.model = ketQuaXSModel.baseJson();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        } else {
          console.log("LOG HANLE CREATE: ")
          /*  let currentDate = new Date(this.model.date);
           currentDate.setHours(currentDate.getHours() + 8);
           this.$set(this.model, 'date', currentDate.toISOString()); */
          // Create model
          await this.$store.dispatch("ketQuaStore/create", this.model).then((res) => {
            if (res.code === 0) {
              this.showModal = false;
              this.getKQXoSo();
              this.model = ketQuaXSModel.baseJson();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        }
        loader.hide();

      }
      this.submitted = false;
    },
    async getKQXoSo() {
      await this.$store.dispatch("ketQuaStore/getbydate", this.model.date).then((res) => {
        if (res.data == null) {
          this.model = ketQuaXSModel.baseJson();
        } else {
          this.model = res.data;
          this.model.date = new Date(res.data.date);
          this.fnGetList();
        }
      });
    },
    async fnGetList() {
      this.$refs.tblList?.refresh()
    },
    dateDisabled(ymd, date) {
      // Disable weekends (Sunday = `0`, Saturday = `6`) and
      // disable days that fall on the 13th of the month
      const weekday = date.getDay()
      const day = date.getDate()
      // Return `true` if the date should be disabled
      return weekday !== 1 
    }
  },
  computed: {

  },
  watch: {
    showModal(status) {
      if (status == false) this.model = ketQuaXSModel.baseJson();
    },
    model() {
      return this.model;
    }
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items" />
    <div class="row mb-3">
      <div class="col-12">
        <label class="text-left" style="width: 100px;">Ngày xổ số</label>
        <div class="row">        
          <div class="col-8">
            <b-form-datepicker
              :date-disabled-fn="dateDisabled" 
              :date-format-options="{ year: 'numeric', month: 'numeric', day: 'numeric' }" 
              :locale="locale" 
              id="date-fi" 
              v-model="model.date" 
              class="mb-2"
              placeholder="Chọn ngày"
            >
            </b-form-datepicker>
          </div>
          <div class="col-4">
            <b-button type="button" variant="danger" @click="clearDate" style="height: 36px; margin-left: 5px !important;">
              Xóa
            </b-button>
            <b-button type="button" variant="warning" class="ms-1" v-on:click="getKQXoSo">
              Tìm
            </b-button>
          </div>
        </div>
      </div>
    </div>
    <!--    <div class="card">-->
    <!--      <div style="display: flex; align-items: center;">-->
    <!--        <label class="text-left" style="width: 100px;">Ngày xổ số: </label>-->
    <!--        <date-picker v-model="model.date" format="DD/MM/YYYY" id="date" placeholder="Nhập ngày xổ số"></date-picker>-->
    <!--        <b-button type="button" variant="warning" class="ms-1" v-on:click="getKQXoSo">-->
    <!--          Tìm-->
    <!--        </b-button>-->
    <!--      </div>-->
    <!--    </div>-->

    <div class="row">
      <div class="col-md-6 col-sm-12">
        <div class="card">
          <div class="card-body">
            <p class="img-kqxs" v-if="model.file">
              <b-card-img :src="apiView + model.file.fileId" class="rounded-0" :height="700" :width="250"></b-card-img>
            </p>
          </div>
        </div>
      </div>
      <div class="col-md-6 col-sm-12">
        <div class="card">
          <div class="card-body">
            <form @submit.prevent="handleSubmit" ref="formContainer">
              <div class="row">
                <div class="col-12">
                  <!-- <div class="mb-3" style="display: flex; justify-content: space-between; align-items: center;">
                    <label class="text-left">Load tự động</label>
                    <switches class="mt-2 mb-0"  v-model="model.dongBo" type-bold="false" color="success" ></switches>
                  </div> -->
                  <div class="mb-3">
                    <label class="text-left text-black font-weight-bold">Ngày xổ số</label>
                    <span style="color: red">&nbsp;*</span>   
                    <b-form-datepicker
                      :date-disabled-fn="dateDisabled" 
                      :date-format-options="{ year: 'numeric', month: 'numeric', day: 'numeric' }"  
                      :locale="locale" 
                      id="date" 
                      v-model="model.date" 
                      class="mb-2"
                      placeholder="Chọn ngày"
                    >
                    </b-form-datepicker>                 
                    <!-- <date-picker v-model="model.date" format="DD/MM/YYYY" id="date" placeholder="Nhập ngày xổ số">
                    </date-picker> -->
                    <div v-if="submitted && !$v.model.date.required" class="invalid-feedback">
                      Ngày xổ số không được trống.
                    </div>
                  </div>
                  <div class="mb-3">
                    <label class="text-left text-black font-weight-bold">Kỳ vé</label>
                    <input id="giai8" v-model="model.kyVe" type="text" class="form-control"
                      placeholder="" />
                  </div>
                  <div class="mb-3">
                    <label class="text-left text-black font-weight-bold">Giải tám</label>
                    <input id="giai8" v-model="model.giai8" type="text" class="form-control"
                      placeholder="" />
                  </div>
                  <div class="mb-3">
                    <label class="text-left text-black font-weight-bold">Giải bảy</label>
                    <input id="giai7" v-model="model.giai7" type="text" class="form-control"
                      placeholder="" />
                  </div>
                  <label class="text-left text-black font-weight-bold">Giải sáu</label>
                  <div class="mb-3" style="display: flex; justify-content: space-between;">
                    <input v-for="(value, index) in model.giai6" :key="index" v-model="model.giai6[index]" type="text"
                      class="form-control me-2" :placeholder="''" />
                  </div>
                  <div class="mb-3">
                    <label class="text-left text-black font-weight-bold">Giải năm</label>
                    <input id="giai5" v-model="model.giai5" type="text" class="form-control"
                      placeholder="" />
                  </div>
                  <label class="text-left text-black font-weight-bold">Giải tư</label>
                  <div class="mb-3 w-giai4" style="display: flex; justify-content: space-between;">
                    <input v-for="(value, index) in model.giai4" :key="index" v-model="model.giai4[index]" type="text"
                      class="form-control me-1 wi-giai4" :placeholder="''" />
                  </div>
                  <label class="text-left text-black font-weight-bold">Giải ba</label>
                  <div class="mb-3" style="display: flex; justify-content: space-between;">
                    <input v-for="(value, index) in model.giai3" :key="index" v-model="model.giai3[index]" type="text"
                      class="form-control me-2" :placeholder="''" />
                  </div>
                  <div class="mb-3">
                    <label class="text-left text-black font-weight-bold">Giải nhì</label>
                    <input id="giai2" v-model="model.giai2" type="text" class="form-control"
                      placeholder="" />
                  </div>
                  <div class="mb-3">
                    <label class="text-left text-black font-weight-bold">Giải nhất</label>
                    <input id="giai1" v-model="model.giai1" type="text" class="form-control"
                      placeholder="" />
                  </div>
                  <div class="mb-3">
                    <label class="text-left text-black font-weight-bold">Giải đặc biệt</label>
                    <input id="giaiDB" v-model="model.giaiDB" type="text" class="form-control"
                      placeholder="" />
                  </div>

                </div>
              </div>
              <div class="text-end pt-2 mt-3">
                <b-button type="button" variant="warning" class="ms-1" v-on:click="handleResetForm"> 
                  Tạo vé số
                </b-button>

                <b-button 
                  v-if="$can('manage-KETQUAXOSO-656004cf4235b5c9c4c589e9') || $can('create-KETQUAXOSO-656004cf4235b5c9c4c589e9')"
                  type="submit" 
                  variant="success" 
                  class="ms-1 cs-btn-primary px-3">
                  Lưu kết quả xổ số
                </b-button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>
<style>
.mx-table .mx-table-date .table thead th,
thead,
th {
  background-color: rgb(255, 255, 255) !important;
  accent-color: #0e0e0e !important;
  border: none;
  color: #0e0e0e !important;
}

.mx-calendar-content .cell.active {
  color: #fff;
  background-color: #1284e7 !important;
}
.dropdown-menu.show{
  width: 100%;
}
.b-calendar-inner{
  width: 100% !important;
}
.b-calendar-grid-caption{
  display: none;
}

.img-kqxs img{
  height: 100%;
}

.w-giai4{
  width: 100%;
}


</style>
