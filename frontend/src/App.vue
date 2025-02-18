<template>
  <v-app class="rounded rounded-md">
    <v-app-bar :elevation="2">
      <v-app-bar-title>{{ t('title') }}</v-app-bar-title>
      
      <v-spacer />

      <v-btn class="text-none" stacked>
        <v-avatar 
          :color="connectedColor" 
          size="medium"
          :title="connectedTitle"
        >
          <v-icon>{{ connectedIcon }}</v-icon>
        </v-avatar>
      </v-btn>
    </v-app-bar>
    <v-main 
      class="d-flex align-center justify-center" 
      style="min-height: 300px;"
    >
      <div>
        <v-card
          v-if="!state.prop"
          class="mx-auto"
          prepend-icon="mdi-alert"
          color="red"
          dark
          width="400"
        >
          <template #title>
            <span class="font-weight-black">ProPresenter Offline</span>
          </template>
          <v-card-text>            
            ProPresenter is aktuell nicht verbunden.<br>Bitte kontaktiere den Producer direkt.
          </v-card-text>
        </v-card>
        <v-container v-if="state.prop">
          <v-form @submit.prevent="submit()">
            <v-row>
              <v-col 
                cols="12" 
                md="12"
              >
                <v-text-field
                  v-model="code"
                  size="10"
                  :rules="codeRules"
                  :label="t('code')"
                  required
                />
              </v-col>
            </v-row>
            <v-row>
              <v-col 
                cols="12" 
                md="12"
              >
                <v-btn
                  type="submit"
                  class="mt-2"
                  :text="t('submit')"
                  :disabled="!valid"
                  block
                />
              </v-col>
            </v-row>
          </v-form>
        </v-container>

        <v-container>
          <v-row>
            <v-col 
              cols="12" 
              md="12"
            >
              <v-data-table
                :sort-by="sortBy"
                :headers="headers"
                :items="entries"
                class="elevation-1"
              >
                <template #item.timeStamp="{ item }">
                  <span>{{ formatRelativeDate(item.timeStamp) }}</span>
                </template>
                <template #item.state="{ item }">
                  <v-chip :color="getColor(item.state)">{{ t(item.state) }}</v-chip>
                </template>
              </v-data-table>
            </v-col>
          </v-row>
        </v-container>
      </div>
    </v-main>
    <v-footer />
  </v-app>
</template>

<script>
import { io } from 'socket.io-client';
import { formatRelative } from 'date-fns';

import { de, enGB } from "date-fns/locale";
const dateLocales = { de, en: enGB };

const serverPath = (import.meta.env.VITE_BACKEND_PATH) ? import.meta.env.VITE_BACKEND_PATH : '/';

const messages = {
  de: {
    title: 'ICF ProPresenter Kids Call',
    code: 'Code',
    codeRules: 'Code ist erforderlich.',
    codeRulesLength: 'Code muss weniger als 10 Zeichen lang sein.',
    submit: 'Senden',
    timestamp: 'Datum/Zeit',
    status: 'Status',
    created: 'Erstellt',
    showing: 'Wird angezeigt',
    overwritten: 'Überschrieben',
    declined: 'Abgelehnt',
    done: 'Erledigt',
    'prop.connected': 'ProPresenter Online',
    'prop.disconnected': 'ProPresenter Offline',
  },
  en: {
    title: 'ICF ProPresenter Kids Call',
    code: 'Code',
    codeRules: 'Code is required.',
    codeRulesLength: 'Code must be less than 10 characters.',
    submit: 'Submit',
    timestamp: 'Date/Time',
    status: 'Status',
    created: 'Created',
    showing: 'Showing',
    overwritten: 'Overwritten',
    declined: 'Declined',
    done: 'Done',
    'prop.connected': 'ProPresenter Online',
    'prop.disconnected': 'ProPresenter Offline',
  },
}

function getLocale() {
  if (navigator.languages && navigator.languages.length && navigator.languages[0].startsWith('de')) return 'de'
  else return 'en'
}

function translate(key) {
  const texts = messages[getLocale()]
  return texts && texts[key] ? texts[key] : key
}

export default {
  data: () => ({
    code: '',
    entries: [],
    state: {
      prop: false,
    },
    codeRules: [
      value => {
        if (value) return true
        return translate('codeRules')
      },
      value => {
        if (value?.length <= 10) return true
        return translate('codeRulesLength')
      },
    ],
    sortBy: [ { key: 'timeStamp', order: 'desc' } ],
    headers: [
      { title: translate('code'), key: 'code' },
      { title: translate('timestamp'), key: 'timeStamp' },
      { title: translate('status'), value: 'state' },
    ],
  }),

  computed: {
    valid() {
      return this.codeRules.every(rule => rule(this.code) === true);
    },
    connectedColor() {
      return this.state.prop ? 'green' : 'red';
    },
    connectedIcon() {
      return this.state.prop ? 'mdi-check' : 'mdi-close';
    },
    connectedTitle() {
      return this.state.prop ? this.t('prop.connected') : this.t('prop.disconnected');
    },
  },

  created() {
    this.socket = io(serverPath, { autoConnect: false });

    this.socket.on('entry', (data) => {
      if (data.id) {
        const index = this.entries.findIndex(entry => entry.id === data.id);
        if (index !== -1) {
          Object.assign(this.entries[index], data);
        } else {
          this.entries.push(data);
        }
      }
    });

    this.socket.on('entries', (data) => {
      this.entries.splice(0, this.entries.length);
      this.entries.push(...data);
    });

    this.socket.on('connect', () => {
      console.log('Connected to server');
    });

    this.socket.on('state', (data) => {
      Object.assign(this.state, data);
    });

    this.socket.connect();
  },

  methods: {
    submit() {
      this.socket.emit('entry', { code: this.code });
      this.code = '';
    },
    t(key) {
      return translate(key)
    },
    formatRelativeDate(date) {
      return formatRelative(new Date(date), new Date(), { locale: dateLocales[getLocale()] },);
    },
    getColor (state) {
      if (state === 'created') return 'yellow'
      else if (state == 'showing') return 'green'
      else if (state == 'done') return 'green'
      else if (state == 'desclined') return 'red'
      else if (state == 'overwritten') return 'orange'
      else return 'grey'
    },
  },
}
</script>
